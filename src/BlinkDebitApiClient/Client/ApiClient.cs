/*
 * Copyright (c) 2023 BlinkPay
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BlinkDebitApiClient.Client.Auth;
using BlinkDebitApiClient.Config;
using BlinkDebitApiClient.Enums;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using RestSharp;
using RestSharp.Serializers;
using RestSharpMethod = RestSharp.Method;

namespace BlinkDebitApiClient.Client;

/// <summary>
/// Allows RestSharp to Serialize/Deserialize JSON using our custom logic, but only when ContentType is JSON.
/// </summary>
internal class CustomJsonCodec : IRestSerializer, ISerializer, IDeserializer
{
    private readonly IReadableConfiguration _configuration;

    private static readonly ContentType _contentType = ContentType.Json;

    private readonly JsonSerializerSettings _serializerSettings = new()
    {
        // OpenAPI generated types generally hide default constructors.
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                OverrideSpecifiedNames = false
            }
        }
    };

    public CustomJsonCodec(IReadableConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CustomJsonCodec(JsonSerializerSettings serializerSettings, IReadableConfiguration configuration)
    {
        _serializerSettings = serializerSettings;
        _configuration = configuration;
    }

    /// <summary>
    /// Serialize the object into a JSON string.
    /// </summary>
    /// <param name="obj">Object to be serialized.</param>
    /// <returns>A JSON string.</returns>
    public string Serialize(object obj)
    {
        if (obj != null && obj is AbstractOpenAPISchema)
        {
            // the object to be serialized is an oneOf/anyOf schema
            return ((AbstractOpenAPISchema)obj).ToJson();
        }

        return JsonConvert.SerializeObject(obj, _serializerSettings);
    }

    public string Serialize(Parameter bodyParameter) => Serialize(bodyParameter.Value);

    public T Deserialize<T>(RestResponse response)
    {
        var result = (T)Deserialize(response, typeof(T));
        return result;
    }

    /// <summary>
    /// Deserialize the JSON string into a proper object.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="type">Object type.</param>
    /// <returns>Object representation of the JSON string.</returns>
    private object Deserialize(RestResponse response, Type type)
    {
        if (type == typeof(byte[])) // return byte array
        {
            return response.RawBytes;
        }

        // TODO: ? if (type.IsAssignableFrom(typeof(Stream)))
        if (type == typeof(Stream))
        {
            var bytes = response.RawBytes;
            if (response.Headers != null)
            {
                var filePath = string.IsNullOrEmpty(_configuration.TempFolderPath)
                    ? Path.GetTempPath()
                    : _configuration.TempFolderPath;
                var regex = new Regex(@"Content-Disposition=.*filename=['""]?([^'""\s]+)['""]?$");
                foreach (var header in response.Headers)
                {
                    var match = regex.Match(header.ToString());
                    if (match.Success)
                    {
                        var fileName = filePath +
                                       ClientUtils.SanitizeFilename(match.Groups[1].Value.Replace("\"", "")
                                           .Replace("'", ""));
                        File.WriteAllBytes(fileName, bytes);
                        return new FileStream(fileName, FileMode.Open);
                    }
                }
            }

            var stream = new MemoryStream(bytes);
            return stream;
        }

        if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
        {
            return DateTime.Parse(response.Content, null, DateTimeStyles.RoundtripKind);
        }

        if (type == typeof(string) || type.Name.StartsWith("System.Nullable")) // return primitive type
        {
            return Convert.ChangeType(response.Content, type);
        }

        // at this point, it must be a model (json)
        try
        {
            return JsonConvert.DeserializeObject(response.Content, type, _serializerSettings);
        }
        catch (Exception e)
        {
            throw new BlinkInternalServerErrorException(e.Message, e);
        }
    }

    public ISerializer Serializer => this;
    public IDeserializer Deserializer => this;

    public string[] AcceptedContentTypes => ContentType.JsonAccept;

    public SupportsContentType SupportsContentType => contentType =>
        contentType.Value.EndsWith("json", StringComparison.InvariantCultureIgnoreCase) ||
        contentType.Value.EndsWith("javascript", StringComparison.InvariantCultureIgnoreCase);

    public ContentType ContentType
    {
        get => _contentType;
        set => throw new InvalidOperationException("Not allowed to set content type.");
    }

    public DataFormat DataFormat => DataFormat.Json;
}

/// <summary>
/// Provides a default implementation of an Api client (both synchronous and asynchronous implementations),
/// encapsulating general REST accessor use cases.
/// </summary>
public class ApiClient : ISynchronousClient, IAsynchronousClient
{
    private readonly string _baseUrl;

    private readonly ILogger _logger;

    /// <summary>
    /// Specifies the settings on a <see cref="JsonSerializer" /> object.
    /// These settings can be adjusted to accommodate custom serialization rules.
    /// </summary>
    private JsonSerializerSettings SerializerSettings { get; } = new()
    {
        // OpenAPI generated types generally hide default constructors.
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                OverrideSpecifiedNames = false
            }
        }
    };

    /// <summary>
    /// Allows for extending request processing for <see cref="ApiClient"/> generated code.
    /// </summary>
    /// <param name="request">The RestSharp request object</param>
    /// <param name="options">The RequestOptions</param>
    private void InterceptRequest(RestRequest request, RequestOptions options)
    {
        var present = options.HeaderParameters.ContainsKey(BlinkDebitConstant.CORRELATION_ID.GetValue());
        var correlationId = "";
        if (present)
        {
            correlationId = ClientUtils.ParameterToString(options.HeaderParameters[BlinkDebitConstant.CORRELATION_ID.GetValue()]);
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                options.HeaderParameters.Add(BlinkDebitConstant.CORRELATION_ID.GetValue(),
                    ClientUtils.ParameterToString(correlationId)); // header parameter
            }
        }

        request.OnBeforeDeserialization = resp =>
        {
            using (_logger.BeginScope($"CorrelationId: {correlationId}"))
            {
                var body = request.Parameters
                    .Where(param => param.Type == ParameterType.RequestBody)
                    .Select(param => param.Value)
                    .FirstOrDefault();

                _logger.LogDebug("Action: {method} {url}\nHeaders: {headers}\nBody: {body}", request.Method,
                    request.Resource, SanitiseHeaders(options.HeaderParameters), body);
            }
        };
    }
    
    private static string SanitiseHeaders(Multimap<string, string> headers)
    {
        var map = new Dictionary<string, IList<string>>(headers);

        if (map.TryGetValue(BlinkDebitConstant.AUTHORIZATION.GetValue(), out var authorizations))
        {
            for (int i = 0; i < authorizations.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(authorizations[i]))
                {
                    authorizations[i] = "***REDACTED BEARER TOKEN***";
                    break;
                }
            }
        }

        return string.Join(",", map.Select(kvp =>
        {
            var argValue = ClientUtils.ParameterToString(kvp.Value);
            return $"{kvp.Key}:{argValue}";
        }));
    }

    /// <summary>
    /// Allows for extending response processing for <see cref="ApiClient"/> generated code.
    /// </summary>
    /// <param name="response">The RestSharp response object</param>
    private void InterceptResponse(RestResponseBase response)
    {
        var correlationId = response.Headers?.SingleOrDefault(h =>
                h.Name.Equals(BlinkDebitConstant.CORRELATION_ID.GetValue(), StringComparison.OrdinalIgnoreCase))?.Value
            .ToString();

        using (_logger.BeginScope($"CorrelationId: {correlationId}"))
        {
            _logger.LogDebug("Status Code: {code}\nHeaders: {headers}\nBody: {body}", response.StatusCode,
                string.Join(", ", response.Headers), response.Content);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient" />, defaulting to the global configurations' base url.
    /// </summary>
    /// <param name="logger">The logger</param>
    public ApiClient(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _baseUrl = GlobalConfiguration.Instance.BasePath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient" />
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="basePath">The target service's base path in URL format.</param>
    /// <exception cref="ArgumentException"></exception>
    public ApiClient(ILogger logger, string basePath)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (string.IsNullOrEmpty(basePath))
            throw new ArgumentException("basePath cannot be empty");

        _baseUrl = basePath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient" />
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="configuration">The configuration</param>
    /// <exception cref="ArgumentException">Thrown when base path is null or empty</exception>
    public ApiClient(ILogger logger, IReadableConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (string.IsNullOrEmpty(configuration.BasePath))
            throw new ArgumentException("basePath cannot be empty");

        _baseUrl = configuration.BasePath;
        configuration.Authenticator ??= new OAuthAuthenticator(configuration.OAuthTokenUrl,
            configuration.OAuthClientId,
            configuration.OAuthClientSecret, configuration.OAuthFlow, SerializerSettings, configuration);
    }

    /// <summary>
    /// Constructs the RestSharp version of an http method
    /// </summary>
    /// <param name="method">Swagger Client Custom HttpMethod</param>
    /// <returns>RestSharp's HttpMethod instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private RestSharpMethod Method(HttpMethod method)
    {
        RestSharpMethod other;
        switch (method)
        {
            case HttpMethod.Get:
                other = RestSharpMethod.Get;
                break;
            case HttpMethod.Post:
                other = RestSharpMethod.Post;
                break;
            case HttpMethod.Put:
                other = RestSharpMethod.Put;
                break;
            case HttpMethod.Delete:
                other = RestSharpMethod.Delete;
                break;
            case HttpMethod.Head:
                other = RestSharpMethod.Head;
                break;
            case HttpMethod.Options:
                other = RestSharpMethod.Options;
                break;
            case HttpMethod.Patch:
                other = RestSharpMethod.Patch;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }

        return other;
    }

    /// <summary>
    /// Provides all logic for constructing a new RestSharp <see cref="RestRequest"/>.
    /// At this point, all information for querying the service is known. Here, it is simply
    /// mapped into the RestSharp request.
    /// </summary>
    /// <param name="method">The http verb.</param>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>[private] A new RestRequest instance.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private RestRequest NewRequest(HttpMethod method, string path, RequestOptions options,
        IReadableConfiguration configuration)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (options == null) throw new ArgumentNullException(nameof(options));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var request = new RestRequest(path, Method(method));

        if (options.PathParameters != null)
        {
            foreach (var pathParam in options.PathParameters)
            {
                request.AddParameter(pathParam.Key, pathParam.Value, ParameterType.UrlSegment);
            }
        }

        if (options.QueryParameters != null)
        {
            foreach (var queryParam in options.QueryParameters)
            {
                foreach (var value in queryParam.Value)
                {
                    request.AddQueryParameter(queryParam.Key, value);
                }
            }
        }

        if (configuration.DefaultHeaders != null)
        {
            foreach (var headerParam in configuration.DefaultHeaders)
            {
                request.AddHeader(headerParam.Key, headerParam.Value);
            }
        }

        if (options.HeaderParameters != null)
        {
            foreach (var headerParam in options.HeaderParameters)
            {
                foreach (var value in headerParam.Value)
                {
                    request.AddHeader(headerParam.Key, value);
                }
            }
        }

        if (options.FormParameters != null)
        {
            foreach (var formParam in options.FormParameters)
            {
                request.AddParameter(formParam.Key, formParam.Value);
            }
        }

        if (options.Data != null)
        {
            if (options.Data is Stream stream)
            {
                var contentType = "application/octet-stream";
                if (options.HeaderParameters != null)
                {
                    var contentTypes = options.HeaderParameters["Content-Type"];
                    contentType = contentTypes[0];
                }

                var bytes = ClientUtils.ReadAsBytes(stream);
                request.AddParameter(contentType, bytes, ParameterType.RequestBody);
            }
            else
            {
                if (options.HeaderParameters != null)
                {
                    var contentTypes = options.HeaderParameters["Content-Type"];
                    if (contentTypes == null || contentTypes.Any(header => header.Contains("application/json")))
                    {
                        request.RequestFormat = DataFormat.Json;
                    }
                    // TODO: Generated client user should add additional handlers. RestSharp only supports XML and JSON, with XML as default.
                }
                else
                {
                    // Here, we'll assume JSON APIs are more common. XML can be forced by adding produces/consumes to openapi spec explicitly.
                    request.RequestFormat = DataFormat.Json;
                }

                request.AddJsonBody(options.Data);
            }
        }

        if (options.FileParameters != null)
        {
            foreach (var fileParam in options.FileParameters)
            {
                foreach (var file in fileParam.Value)
                {
                    var bytes = ClientUtils.ReadAsBytes(file);
                    var fileStream = file as FileStream;
                    if (fileStream != null)
                        request.AddFile(fileParam.Key, bytes, Path.GetFileName(fileStream.Name));
                    else
                        request.AddFile(fileParam.Key, bytes, "no_file_name_provided");
                }
            }
        }

        return request;
    }

    private ApiResponse<T> ToApiResponse<T>(RestResponse<T> response)
    {
        var result = response.Data;
        var rawContent = response.Content;

        var transformed = new ApiResponse<T>(response.StatusCode, new Multimap<string, string>(), result, rawContent)
        {
            ErrorText = response.ErrorMessage,
            Cookies = new List<Cookie>()
        };

        if (response.Headers != null)
        {
            foreach (var responseHeader in response.Headers)
            {
                transformed.Headers.Add(responseHeader.Name, ClientUtils.ParameterToString(responseHeader.Value));
            }
        }

        if (response.ContentHeaders != null)
        {
            foreach (var responseHeader in response.ContentHeaders)
            {
                transformed.Headers.Add(responseHeader.Name, ClientUtils.ParameterToString(responseHeader.Value));
            }
        }

        if (response.Cookies != null)
        {
            foreach (var responseCookies in response.Cookies.Cast<Cookie>())
            {
                transformed.Cookies.Add(
                    new Cookie(
                        responseCookies.Name,
                        responseCookies.Value,
                        responseCookies.Path,
                        responseCookies.Domain)
                );
            }
        }

        return transformed;
    }

    private ApiResponse<T> Exec<T>(RestRequest req, RequestOptions options, IReadableConfiguration configuration)
    {
        var baseUrl = configuration.GetOperationServerUrl(options.Operation, options.OperationIndex) ?? _baseUrl;

        var cookies = new CookieContainer();

        if (options.Cookies != null && options.Cookies.Count > 0)
        {
            foreach (var cookie in options.Cookies)
            {
                cookies.Add(new Cookie(cookie.Name, cookie.Value));
            }
        }

        var clientOptions = new RestClientOptions(baseUrl)
        {
            ClientCertificates = configuration.ClientCertificates,
            CookieContainer = cookies,
            MaxTimeout = configuration.Timeout,
            Proxy = configuration.Proxy,
            UserAgent = configuration.UserAgent,
            Authenticator = configuration.Authenticator
        };

        var client = new RestClient(clientOptions,
            configureSerialization: s => s.UseSerializer(() => new CustomJsonCodec(SerializerSettings, configuration)));

        InterceptRequest(req, options);

        RestResponse<T> response;
        if (RetryConfiguration.RetryPolicy != null)
        {
            var policy = RetryConfiguration.RetryPolicy;
            var policyResult = policy.ExecuteAndCapture(() => client.Execute(req));
            response = (policyResult.Outcome == OutcomeType.Successful)
                ? client.Deserialize<T>(policyResult.Result)
                : new RestResponse<T>(req)
                {
                    ErrorException = policyResult.FinalException
                };
        }
        else
        {
            response = client.Execute<T>(req);
        }

        // if the response type is oneOf/anyOf, call FromJSON to deserialize the data
        if (typeof(AbstractOpenAPISchema).IsAssignableFrom(typeof(T)))
        {
            try
            {
                response.Data = (T)typeof(T).GetMethod("FromJson").Invoke(null, new object[] { response.Content });
            }
            catch (Exception ex)
            {
                throw ex.InnerException != null ? ex.InnerException : ex;
            }
        }
        else if (typeof(T).Name == "Stream") // for binary response
        {
            response.Data = (T)(object)new MemoryStream(response.RawBytes);
        }
        else if (typeof(T).Name == "Byte[]") // for byte response
        {
            response.Data = (T)(object)response.RawBytes;
        }
        else if (typeof(T).Name == "String") // for string response
        {
            response.Data = (T)(object)response.Content;
        }

        InterceptResponse(response);

        var result = ToApiResponse(response);
        if (response.ErrorMessage != null)
        {
            result.ErrorText = response.ErrorMessage;
        }

        if (response.Cookies != null && response.Cookies.Count > 0)
        {
            if (result.Cookies == null) result.Cookies = new List<Cookie>();
            foreach (var restResponseCookie in response.Cookies.Cast<Cookie>())
            {
                var cookie = new Cookie(
                    restResponseCookie.Name,
                    restResponseCookie.Value,
                    restResponseCookie.Path,
                    restResponseCookie.Domain
                )
                {
                    Comment = restResponseCookie.Comment,
                    CommentUri = restResponseCookie.CommentUri,
                    Discard = restResponseCookie.Discard,
                    Expired = restResponseCookie.Expired,
                    Expires = restResponseCookie.Expires,
                    HttpOnly = restResponseCookie.HttpOnly,
                    Port = restResponseCookie.Port,
                    Secure = restResponseCookie.Secure,
                    Version = restResponseCookie.Version
                };

                result.Cookies.Add(cookie);
            }
        }

        return result;
    }

    private async Task<ApiResponse<T>> ExecAsync<T>(RestRequest req, RequestOptions options,
        IReadableConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken))
    {
        var baseUrl = configuration.GetOperationServerUrl(options.Operation, options.OperationIndex) ?? _baseUrl;

        var clientOptions = new RestClientOptions(baseUrl)
        {
            ClientCertificates = configuration.ClientCertificates,
            MaxTimeout = configuration.Timeout,
            Proxy = configuration.Proxy,
            UserAgent = configuration.UserAgent,
            Authenticator = configuration.Authenticator
        };

        var client = new RestClient(clientOptions,
            configureSerialization: s => s.UseSerializer(() => new CustomJsonCodec(SerializerSettings, configuration)));

        InterceptRequest(req, options);

        RestResponse<T> response;
        if (RetryConfiguration.AsyncRetryPolicy != null)
        {
            var policy = RetryConfiguration.AsyncRetryPolicy;
            var policyResult = await policy
                .ExecuteAndCaptureAsync(ct => client.ExecuteAsync(req, ct), cancellationToken).ConfigureAwait(false);
            response = policyResult.Outcome == OutcomeType.Successful
                ? client.Deserialize<T>(policyResult.Result)
                : new RestResponse<T>(req)
                {
                    ErrorException = policyResult.FinalException
                };
        }
        else
        {
            response = await client.ExecuteAsync<T>(req, cancellationToken).ConfigureAwait(false);
        }

        // if the response type is oneOf/anyOf, call FromJSON to deserialize the data
        if (typeof(AbstractOpenAPISchema).IsAssignableFrom(typeof(T)))
        {
            response.Data = (T)typeof(T).GetMethod("FromJson").Invoke(null, new object[] { response.Content });
        }
        else if (typeof(T).Name == "Stream") // for binary response
        {
            response.Data = (T)(object)new MemoryStream(response.RawBytes);
        }
        else if (typeof(T).Name == "Byte[]") // for byte response
        {
            response.Data = (T)(object)response.RawBytes;
        }

        InterceptResponse(response);

        var result = ToApiResponse(response);
        if (response.ErrorMessage != null)
        {
            result.ErrorText = response.ErrorMessage;
        }

        if (response.Cookies != null && response.Cookies.Count > 0)
        {
            if (result.Cookies == null) result.Cookies = new List<Cookie>();
            foreach (var restResponseCookie in response.Cookies.Cast<Cookie>())
            {
                var cookie = new Cookie(
                    restResponseCookie.Name,
                    restResponseCookie.Value,
                    restResponseCookie.Path,
                    restResponseCookie.Domain
                )
                {
                    Comment = restResponseCookie.Comment,
                    CommentUri = restResponseCookie.CommentUri,
                    Discard = restResponseCookie.Discard,
                    Expired = restResponseCookie.Expired,
                    Expires = restResponseCookie.Expires,
                    HttpOnly = restResponseCookie.HttpOnly,
                    Port = restResponseCookie.Port,
                    Secure = restResponseCookie.Secure,
                    Version = restResponseCookie.Version
                };

                result.Cookies.Add(cookie);
            }
        }

        return result;
    }

    #region IAsynchronousClient

    /// <summary>
    /// Make a HTTP GET request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> GetAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Get, path, options, config), options, config, cancellationToken);
    }

    /// <summary>
    /// Make a HTTP POST request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> PostAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Post, path, options, config), options, config, cancellationToken);
    }

    /// <summary>
    /// Make a HTTP PUT request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> PutAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Put, path, options, config), options, config, cancellationToken);
    }

    /// <summary>
    /// Make a HTTP DELETE request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> DeleteAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Delete, path, options, config), options, config, cancellationToken);
    }

    /// <summary>
    /// Make a HTTP HEAD request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> HeadAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Head, path, options, config), options, config, cancellationToken);
    }

    /// <summary>
    /// Make a HTTP OPTION request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> OptionsAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Options, path, options, config), options, config, cancellationToken);
    }

    /// <summary>
    /// Make a HTTP PATCH request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<ApiResponse<T>> PatchAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return ExecAsync<T>(NewRequest(HttpMethod.Patch, path, options, config), options, config, cancellationToken);
    }

    #endregion IAsynchronousClient

    #region ISynchronousClient

    /// <summary>
    /// Make a HTTP GET request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Get<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Get, path, options, config), options, config);
    }

    /// <summary>
    /// Make a HTTP POST request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Post<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Post, path, options, config), options, config);
    }

    /// <summary>
    /// Make a HTTP PUT request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Put<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Put, path, options, config), options, config);
    }

    /// <summary>
    /// Make a HTTP DELETE request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Delete<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Delete, path, options, config), options, config);
    }

    /// <summary>
    /// Make a HTTP HEAD request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Head<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Head, path, options, config), options, config);
    }

    /// <summary>
    /// Make a HTTP OPTION request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Options<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Options, path, options, config), options, config);
    }

    /// <summary>
    /// Make a HTTP PATCH request (synchronous).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
    /// GlobalConfiguration has been done before calling this method.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public ApiResponse<T> Patch<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
    {
        var config = configuration ?? GlobalConfiguration.Instance;
        return Exec<T>(NewRequest(HttpMethod.Patch, path, options, config), options, config);
    }

    #endregion ISynchronousClient
}