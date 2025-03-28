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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using BlinkDebitApiClient.Client;
using BlinkDebitApiClient.Client.Auth;
using BlinkDebitApiClient.Enums;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp.Authenticators;

namespace BlinkDebitApiClient.Config;

/// <summary>
/// Represents a set of configuration settings
/// </summary>
public class Configuration : IReadableConfiguration
{
    #region Constants

    /// <summary>
    /// Identifier for ISO 8601 DateTime Format
    /// </summary>
    /// <remarks>See https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8 for more information.</remarks>
    // ReSharper disable once InconsistentNaming
    public const string ISO8601_DATETIME_FORMAT = "o";

    #endregion Constants

    #region Static Members

    /// <summary>
    /// Default creation of exceptions for a given method name and response object
    /// </summary>
    public static readonly ExceptionFactory DefaultExceptionFactory = (methodName, response, logger) =>
    {
        var status = (int)response.StatusCode;
        if (status < 400) return null;

        var body = JsonConvert.DeserializeObject<DetailErrorResponseModel>(response.RawContent);

        switch (status)
        {
            case 401:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkUnauthorisedException(body.Message);
            case 403:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkForbiddenException(body.Message);
            case 404:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkResourceNotFoundException(body.Message);
            case 408:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkRetryableException(body.Message);
            case 422:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkUnauthorisedException(body.Message);
            case 429:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkRateLimitExceededException(body.Message);
            case 501:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkNotImplementedException(body.Message);
            case 502:
                return new BlinkServiceException($"Service call to Blink Debit failed with error: " +
                                                 $"{body.Message}, please contact BlinkPay with the correlation ID: " +
                                                 $"{response.Headers[BlinkDebitConstant.CORRELATION_ID.GetValue()]}");
            case >= 400 and < 500:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkClientException(body.Message);
            case >= 500:
                logger.LogError("Status Code: {status}\nHeaders: {headers}\nBody: {body}", status,
                    SanitiseHeaders(response.Headers), body.Message);
                throw new BlinkRetryableException(body.Message);
        }

        return null;
    };
    
    private static string SanitiseHeaders(Multimap<string, string> headers)
    {
        var map = new Dictionary<string, IList<string>>(headers);

        return string.Join(",", map.Select(kvp =>
        {
            var argValue = ClientUtils.ParameterToString(kvp.Value);
            return $"{kvp.Key}:{argValue}";
        }));
    }

    #endregion Static Members

    #region Private Members

    /// <summary>
    /// Defines the base path of the target API server.
    /// Example: http://localhost:3000/v1/
    /// </summary>
    private string _basePath;

    /// <summary>
    /// Gets or sets the API key based on the authentication name.
    /// This is the key and value comprising the "secret" for accessing an API.
    /// </summary>
    /// <value>The API key.</value>
    private IDictionary<string, string> _apiKey;

    /// <summary>
    /// Gets or sets the prefix (e.g. Token) of the API key based on the authentication name.
    /// </summary>
    /// <value>The prefix of the API key.</value>
    private IDictionary<string, string> _apiKeyPrefix;

    private string _dateTimeFormat = ISO8601_DATETIME_FORMAT;
    private string _tempFolderPath = Path.GetTempPath();

    /// <summary>
    /// Gets or sets the servers defined in the OpenAPI spec.
    /// </summary>
    /// <value>The servers</value>
    private IList<IReadOnlyDictionary<string, object>> _servers;

    /// <summary>
    /// Gets or sets the operation servers defined in the OpenAPI spec.
    /// </summary>
    /// <value>The operation servers</value>
    private IReadOnlyDictionary<string, List<IReadOnlyDictionary<string, object>>> _operationServers;

    #endregion Private Members

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public Configuration()
    {
        Proxy = null;
        UserAgent = WebUtility.UrlEncode(".NET/Blink SDK 1.1");
        BasePath = "https://sandbox.debit.blinkpay.co.nz/payments/v1";
        DefaultHeaders = new ConcurrentDictionary<string, string>();
        ApiKey = new ConcurrentDictionary<string, string>();
        ApiKeyPrefix = new ConcurrentDictionary<string, string>();
        Servers = new List<IReadOnlyDictionary<string, object>>()
        {
            new Dictionary<string, object>
            {
                { "url", "https://sandbox.debit.blinkpay.co.nz/payments/v1" },
                { "description", "Sandbox" }
            },
            new Dictionary<string, object>
            {
                { "url", "https://debit.blinkpay.co.nz/payments/v1" },
                { "description", "Production" }
            }
        };
        OperationServers = new Dictionary<string, List<IReadOnlyDictionary<string, object>>>();

        // Setting Timeout has side effects (forces ApiClient creation).
        Timeout = 100000;
        RetryEnabled = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public Configuration(
        IDictionary<string, string> defaultHeaders,
        IDictionary<string, string> apiKey,
        IDictionary<string, string> apiKeyPrefix,
        string basePath = "https://sandbox.debit.blinkpay.co.nz/payments/v1") : this()
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new BlinkInvalidValueException("The basePath cannot be null or empty");
        if (defaultHeaders == null)
            throw new BlinkInvalidValueException("The defaultHeaders cannot be null");
        if (apiKey == null)
            throw new BlinkInvalidValueException("The apiKey cannot be null");
        if (apiKeyPrefix == null)
            throw new BlinkInvalidValueException("The apiKeyPrefix cannot be null");

        BasePath = basePath;

        foreach (var keyValuePair in defaultHeaders)
        {
            DefaultHeaders.Add(keyValuePair);
        }

        foreach (var keyValuePair in apiKey)
        {
            ApiKey.Add(keyValuePair);
        }

        foreach (var keyValuePair in apiKeyPrefix)
        {
            ApiKeyPrefix.Add(keyValuePair);
        }
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the base path for API access.
    /// </summary>
    public virtual string BasePath
    {
        get => _basePath;
        set => _basePath = value;
    }

    /// <summary>
    /// Gets or sets the default header.
    /// </summary>
    [Obsolete("Use DefaultHeaders instead.")]
    public virtual IDictionary<string, string> DefaultHeader
    {
        get => DefaultHeaders;
        set => DefaultHeaders = value;
    }

    /// <summary>
    /// Gets or sets the default headers.
    /// </summary>
    public virtual IDictionary<string, string> DefaultHeaders { get; set; }

    /// <summary>
    /// Gets or sets the HTTP timeout (milliseconds) of ApiClient. Default to 100000 milliseconds.
    /// </summary>
    public virtual int Timeout { get; set; }

    /// <summary>
    /// Gets or sets the proxy
    /// </summary>
    /// <value>Proxy.</value>
    public virtual WebProxy Proxy { get; set; }

    /// <summary>
    /// Gets or sets the HTTP user agent.
    /// </summary>
    /// <value>Http user agent.</value>
    public virtual string UserAgent { get; set; }

    /// <summary>
    /// Gets or sets the username (HTTP basic authentication).
    /// </summary>
    /// <value>The username.</value>
    public virtual string Username { get; set; }

    /// <summary>
    /// Gets or sets the password (HTTP basic authentication).
    /// </summary>
    /// <value>The password.</value>
    public virtual string Password { get; set; }

    /// <summary>
    /// Gets the API key with prefix.
    /// </summary>
    /// <param name="apiKeyIdentifier">API key identifier (authentication scheme).</param>
    /// <returns>API key with prefix.</returns>
    public string GetApiKeyWithPrefix(string apiKeyIdentifier)
    {
        ApiKey.TryGetValue(apiKeyIdentifier, out var apiKeyValue);
        if (ApiKeyPrefix.TryGetValue(apiKeyIdentifier, out var apiKeyPrefix))
        {
            return apiKeyPrefix + " " + apiKeyValue;
        }

        return apiKeyValue;
    }

    /// <summary>
    /// Gets or sets certificate collection to be sent with requests.
    /// </summary>
    /// <value>X509 Certificate collection.</value>
    public X509CertificateCollection ClientCertificates { get; set; }

    /// <summary>
    /// Gets or sets the access token for OAuth2 authentication.
    ///
    /// This helper property simplifies code generation.
    /// </summary>
    /// <value>The access token.</value>
    public virtual string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the token URL for OAuth2 authentication.
    /// </summary>
    /// <value>The OAuth Token URL.</value>
    public virtual string OAuthTokenUrl { get; set; }

    /// <summary>
    /// Gets or sets the client ID for OAuth2 authentication.
    /// </summary>
    /// <value>The OAuth Client ID.</value>
    public virtual string OAuthClientId { get; set; }

    /// <summary>
    /// Gets or sets the client secret for OAuth2 authentication.
    /// </summary>
    /// <value>The OAuth Client Secret.</value>
    public virtual string OAuthClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the flow for OAuth2 authentication.
    /// </summary>
    /// <value>The OAuth Flow.</value>
    public virtual OAuthFlow? OAuthFlow { get; set; }

    /// <summary>
    /// Gets or sets the OAuth2 authenticator.
    /// </summary>
    /// <value>OAuth2 authenticator</value>
    public virtual IAuthenticator? Authenticator { get; set; }

    /// <summary>
    /// Gets or sets the temporary folder path to store the files downloaded from the server.
    /// </summary>
    /// <value>Folder path.</value>
    public virtual string TempFolderPath
    {
        get => _tempFolderPath;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _tempFolderPath = Path.GetTempPath();
                return;
            }

            // create the directory if it does not exist
            if (!Directory.Exists(value))
            {
                Directory.CreateDirectory(value);
            }

            // check if the path contains directory separator at the end
            if (value[value.Length - 1] == Path.DirectorySeparatorChar)
            {
                _tempFolderPath = value;
            }
            else
            {
                _tempFolderPath = value + Path.DirectorySeparatorChar;
            }
        }
    }

    /// <summary>
    /// Gets or sets the date time format used when serializing in the ApiClient
    /// By default, it's set to ISO 8601 - "o", for others see:
    /// https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
    /// and https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
    /// No validation is done to ensure that the string you're providing is valid
    /// </summary>
    /// <value>The DateTimeFormat string</value>
    public virtual string DateTimeFormat
    {
        get => _dateTimeFormat;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                // Never allow a blank or null string, go back to the default
                _dateTimeFormat = ISO8601_DATETIME_FORMAT;
                return;
            }

            // Caution, no validation when you choose date time format other than ISO 8601
            // Take a look at the above links
            _dateTimeFormat = value;
        }
    }

    /// <summary>
    /// Gets or sets the prefix (e.g. Token) of the API key based on the authentication name.
    ///
    /// Whatever you set here will be prepended to the value defined in AddApiKey.
    ///
    /// An example invocation here might be:
    /// <example>
    /// ApiKeyPrefix["Authorization"] = "Bearer";
    /// </example>
    /// … where ApiKey["Authorization"] would then be used to set the value of your bearer token.
    ///
    /// <remarks>
    /// OAuth2 workflows should set tokens via AccessToken.
    /// </remarks>
    /// </summary>
    /// <value>The prefix of the API key.</value>
    public virtual IDictionary<string, string> ApiKeyPrefix
    {
        get => _apiKeyPrefix;
        set
        {
            if (value == null)
            {
                throw new BlinkClientException("ApiKeyPrefix collection may not be null.");
            }

            _apiKeyPrefix = value;
        }
    }

    /// <summary>
    /// Gets or sets the API key based on the authentication name.
    /// </summary>
    /// <value>The API key.</value>
    public virtual IDictionary<string, string> ApiKey
    {
        get => _apiKey;
        set
        {
            if (value == null)
            {
                throw new BlinkClientException("ApiKey collection may not be null.");
            }

            _apiKey = value;
        }
    }

    /// <summary>
    /// Gets or sets the servers.
    /// </summary>
    /// <value>The servers.</value>
    public virtual IList<IReadOnlyDictionary<string, object>> Servers
    {
        get => _servers;
        set
        {
            if (value == null)
            {
                throw new BlinkClientException("Servers may not be null.");
            }

            _servers = value;
        }
    }

    /// <summary>
    /// Gets or sets the operation servers.
    /// </summary>
    /// <value>The operation servers.</value>
    public virtual IReadOnlyDictionary<string, List<IReadOnlyDictionary<string, object>>> OperationServers
    {
        get => _operationServers;
        set
        {
            if (value == null)
            {
                throw new BlinkClientException("Operation servers may not be null.");
            }

            _operationServers = value;
        }
    }

    /// <summary>
    /// Gets the retry flag
    /// </summary>
    /// <value>true if retry is enabled; false otherwise</value>
    public bool RetryEnabled { get; set; }

    /// <summary>
    /// Returns URL based on server settings without providing values
    /// for the variables
    /// </summary>
    /// <param name="index">Array index of the server settings.</param>
    /// <return>The server URL.</return>
    public string GetServerUrl(int index)
    {
        return GetServerUrl(Servers, index, null);
    }

    /// <summary>
    /// Returns URL based on server settings.
    /// </summary>
    /// <param name="index">Array index of the server settings.</param>
    /// <param name="inputVariables">Dictionary of the variables and the corresponding values.</param>
    /// <return>The server URL.</return>
    public string GetServerUrl(int index, Dictionary<string, string> inputVariables)
    {
        return GetServerUrl(Servers, index, inputVariables);
    }

    /// <summary>
    /// Returns URL based on operation server settings.
    /// </summary>
    /// <param name="operation">Operation associated with the request path.</param>
    /// <param name="index">Array index of the server settings.</param>
    /// <return>The operation server URL.</return>
    public string GetOperationServerUrl(string operation, int index)
    {
        return GetOperationServerUrl(operation, index, null);
    }

    /// <summary>
    /// Returns URL based on operation server settings.
    /// </summary>
    /// <param name="operation">Operation associated with the request path.</param>
    /// <param name="index">Array index of the server settings.</param>
    /// <param name="inputVariables">Dictionary of the variables and the corresponding values.</param>
    /// <return>The operation server URL.</return>
    public string GetOperationServerUrl(string operation, int index, Dictionary<string, string> inputVariables)
    {
        return OperationServers.TryGetValue(operation, out var operationServer)
            ? GetServerUrl(operationServer, index, inputVariables)
            : null;
    }

    /// <summary>
    /// Returns URL based on server settings.
    /// </summary>
    /// <param name="servers">Dictionary of server settings.</param>
    /// <param name="index">Array index of the server settings.</param>
    /// <param name="inputVariables">Dictionary of the variables and the corresponding values.</param>
    /// <return>The server URL.</return>
    private string GetServerUrl(IList<IReadOnlyDictionary<string, object>> servers, int index,
        Dictionary<string, string> inputVariables)
    {
        if (index < 0 || index >= servers.Count)
        {
            throw new BlinkClientException(
                $"Invalid index {index} when selecting the server. Must be less than {servers.Count}.");
        }

        if (inputVariables == null)
        {
            inputVariables = new Dictionary<string, string>();
        }

        var server = servers[index];
        var url = (string)server["url"];

        if (server.ContainsKey("variables"))
        {
            // go through each variable and assign a value
            foreach (var variable in (IReadOnlyDictionary<string, object>)server["variables"])
            {
                var serverVariables = (IReadOnlyDictionary<string, object>)(variable.Value);

                if (inputVariables.ContainsKey(variable.Key))
                {
                    if (((List<string>)serverVariables["enum_values"]).Contains(inputVariables[variable.Key]))
                    {
                        url = url.Replace("{" + variable.Key + "}", inputVariables[variable.Key]);
                    }
                    else
                    {
                        throw new BlinkClientException(
                            $"The variable `{variable.Key}` in the server URL has invalid value #{inputVariables[variable.Key]}. Must be {(List<string>)serverVariables["enum_values"]}");
                    }
                }
                else
                {
                    // use default value
                    url = url.Replace("{" + variable.Key + "}", (string)serverVariables["default_value"]);
                }
            }
        }

        return url;
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Returns a string with essential information for debugging.
    /// </summary>
    public static string ToDebugReport()
    {
        var report = ".NET SDK (BlinkDebitApiClient) Debug Report:\n";
        report += "    OS: " + Environment.OSVersion + "\n";
        report += "    .NET Framework Version: " + Environment.Version + "\n";

        return report;
    }

    /// <summary>
    /// Add Api Key Header.
    /// </summary>
    /// <param name="key">Api Key name.</param>
    /// <param name="value">Api Key value.</param>
    /// <returns></returns>
    public void AddApiKey(string key, string value)
    {
        ApiKey[key] = value;
    }

    /// <summary>
    /// Sets the API key prefix.
    /// </summary>
    /// <param name="key">Api Key name.</param>
    /// <param name="value">Api Key value.</param>
    public void AddApiKeyPrefix(string key, string value)
    {
        ApiKeyPrefix[key] = value;
    }

    #endregion Methods

    #region Static Members

    /// <summary>
    /// Merge configurations.
    /// </summary>
    /// <param name="first">First configuration.</param>
    /// <param name="second">Second configuration.</param>
    /// <return>Merged configuration.</return>
    public static IReadableConfiguration MergeConfigurations(IReadableConfiguration first,
        IReadableConfiguration second)
    {
        if (second == null) return first ?? GlobalConfiguration.Instance;

        var apiKey = first.ApiKey.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var apiKeyPrefix = first.ApiKeyPrefix.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var defaultHeaders = first.DefaultHeaders.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var kvp in second.ApiKey) apiKey[kvp.Key] = kvp.Value;
        foreach (var kvp in second.ApiKeyPrefix) apiKeyPrefix[kvp.Key] = kvp.Value;
        foreach (var kvp in second.DefaultHeaders) defaultHeaders[kvp.Key] = kvp.Value;

        var config = new Configuration
        {
            ApiKey = apiKey,
            ApiKeyPrefix = apiKeyPrefix,
            DefaultHeaders = defaultHeaders,
            BasePath = second.BasePath ?? first.BasePath,
            Timeout = second.Timeout,
            Proxy = second.Proxy ?? first.Proxy,
            UserAgent = second.UserAgent ?? first.UserAgent,
            Username = second.Username ?? first.Username,
            Password = second.Password ?? first.Password,
            AccessToken = second.AccessToken ?? first.AccessToken,
            OAuthTokenUrl = second.OAuthTokenUrl ?? first.OAuthTokenUrl,
            OAuthClientId = second.OAuthClientId ?? first.OAuthClientId,
            OAuthClientSecret = second.OAuthClientSecret ?? first.OAuthClientSecret,
            OAuthFlow = second.OAuthFlow ?? first.OAuthFlow,
            TempFolderPath = second.TempFolderPath ?? first.TempFolderPath,
            DateTimeFormat = second.DateTimeFormat ?? first.DateTimeFormat,
            ClientCertificates = second.ClientCertificates ?? first.ClientCertificates,
            Authenticator = second.Authenticator ?? first.Authenticator,
            RetryEnabled = second.RetryEnabled
        };
        return config;
    }

    #endregion Static Members
}