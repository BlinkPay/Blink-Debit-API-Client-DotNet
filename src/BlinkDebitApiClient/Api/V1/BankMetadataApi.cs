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
using System.Threading;
using System.Threading.Tasks;
using BlinkDebitApiClient.Client;
using BlinkDebitApiClient.Config;
using BlinkDebitApiClient.Enums;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Microsoft.Extensions.Logging;

namespace BlinkDebitApiClient.Api.V1;

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IBankMetadataApiSync : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// Get Bank Metadata
    /// </summary>
    /// <remarks>
    /// The available banks, features available for the banks, and relevant fields.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>List&lt;BankMetadata&gt;</returns>
    List<BankMetadata> GetMeta(Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0);

    /// <summary>
    /// Get Bank Metadata
    /// </summary>
    /// <remarks>
    /// The available banks, features available for the banks, and relevant fields.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of List&lt;BankMetadata&gt;</returns>
    ApiResponse<List<BankMetadata>> GetMetaWithHttpInfo(Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0);

    #endregion Synchronous Operations
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IBankMetadataApiAsync : IApiAccessor
{
    #region Asynchronous Operations

    /// <summary>
    /// Get Bank Metadata
    /// </summary>
    /// <remarks>
    /// The available banks, features available for the banks, and relevant fields.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of List&lt;BankMetadata&gt;</returns>
    Task<List<BankMetadata>> GetMetaAsync(Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Get Bank Metadata
    /// </summary>
    /// <remarks>
    /// The available banks, features available for the banks, and relevant fields.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (List&lt;BankMetadata&gt;)</returns>
    Task<ApiResponse<List<BankMetadata>>> GetMetaWithHttpInfoAsync(Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken));

    #endregion Asynchronous Operations
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IBankMetadataApi : IBankMetadataApiSync, IBankMetadataApiAsync
{
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public class BankMetadataApi : IBankMetadataApi
{
    private ExceptionFactory _exceptionFactory = (name, response, logger) => null;

    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataApi"/> class.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="basePath">The base path containing the Blink Debit API URL and the default path (/payments/v1)</param>
    /// <returns></returns>
    public BankMetadataApi(ILogger logger, string basePath)
    {
        _logger = logger ?? throw new BlinkInvalidValueException(nameof(logger) + " cannot be null");
        Configuration = Config.Configuration.MergeConfigurations(
            GlobalConfiguration.Instance,
            new Configuration { BasePath = basePath }
        );
        Client = new ApiClient(logger, Configuration);
        AsynchronousClient = new ApiClient(logger, Configuration);
        ExceptionFactory = Config.Configuration.DefaultExceptionFactory;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataApi"/> class
    /// using Configuration object
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="configuration">An instance of Configuration</param>
    /// <returns></returns>
    public BankMetadataApi(ILogger logger, Configuration configuration)
    {
        _logger = logger ?? throw new BlinkInvalidValueException(nameof(logger) + " cannot be null");
        if (configuration == null) throw new BlinkInvalidValueException(nameof(configuration) + " cannot be null");

        Configuration = Config.Configuration.MergeConfigurations(
            GlobalConfiguration.Instance,
            configuration
        );
        Client = new ApiClient(logger, Configuration);
        AsynchronousClient = new ApiClient(logger, Configuration);
        ExceptionFactory = Config.Configuration.DefaultExceptionFactory;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataApi"/> class
    /// using a Configuration object and client instance.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="client">The client interface for synchronous API access.</param>
    /// <param name="asyncClient">The client interface for asynchronous API access.</param>
    /// <param name="configuration">The configuration object.</param>
    public BankMetadataApi(ILogger logger, ISynchronousClient client, IAsynchronousClient asyncClient,
        IReadableConfiguration configuration)
    {
        _logger = logger ?? throw new BlinkInvalidValueException(nameof(logger) + " cannot be null");
        Client = client ?? throw new BlinkInvalidValueException(nameof(client) + " cannot be null");
        AsynchronousClient =
            asyncClient ?? throw new BlinkInvalidValueException(nameof(asyncClient) + " cannot be null");
        Configuration = configuration ??
                        throw new BlinkInvalidValueException(nameof(configuration) + " cannot be null");
        ExceptionFactory = Config.Configuration.DefaultExceptionFactory;
    }

    /// <summary>
    /// The client for accessing this underlying API asynchronously.
    /// </summary>
    public IAsynchronousClient AsynchronousClient { get; set; }

    /// <summary>
    /// The client for accessing this underlying API synchronously.
    /// </summary>
    public ISynchronousClient Client { get; set; }

    /// <summary>
    /// Gets the base path of the API client.
    /// </summary>
    /// <value>The base path</value>
    public string GetBasePath()
    {
        return Configuration.BasePath;
    }

    /// <summary>
    /// Gets or sets the configuration object
    /// </summary>
    /// <value>An instance of the Configuration</value>
    public IReadableConfiguration Configuration { get; set; }

    /// <summary>
    /// Provides a factory method hook for the creation of exceptions.
    /// </summary>
    public ExceptionFactory ExceptionFactory
    {
        get
        {
            if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
            {
                throw new BlinkClientException("Multicast delegate for ExceptionFactory is unsupported.");
            }

            return _exceptionFactory;
        }
        set => _exceptionFactory = value;
    }

    /// <summary>
    /// Get Bank Metadata The available banks, features available for the banks, and relevant fields.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>List&lt;BankMetadata&gt;</returns>
    public List<BankMetadata> GetMeta(Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0)
    {
        var localVarResponse = GetMetaWithHttpInfo(requestHeaders);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Get Bank Metadata The available banks, features available for the banks, and relevant fields.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of List&lt;BankMetadata&gt;</returns>
    public ApiResponse<List<BankMetadata>> GetMetaWithHttpInfo(Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0)
    {
        var localVarRequestOptions = new RequestOptions();

        var contentTypes = Array.Empty<string>();

        // to determine the Accept header
        var accepts = new[]
        {
            "application/json"
        };

        var localVarContentType = ClientUtils.SelectHeaderContentType(contentTypes);
        localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

        var localVarAccept = ClientUtils.SelectHeaderAccept(accepts);
        localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

        requestHeaders ??= new Dictionary<string, string?>();
        if (requestHeaders.ContainsKey(BlinkDebitConstant.REQUEST_ID.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.REQUEST_ID.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.REQUEST_ID.GetValue())));
        }

        if (requestHeaders.ContainsKey(BlinkDebitConstant.CORRELATION_ID.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.CORRELATION_ID.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.CORRELATION_ID.GetValue())));
        }

        if (requestHeaders.ContainsKey(BlinkDebitConstant.CUSTOMER_IP.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.CUSTOMER_IP.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.CUSTOMER_IP.GetValue())));
        }

        if (requestHeaders.ContainsKey(BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue())));
        }

        localVarRequestOptions.Operation = "BankMetadataApi.GetMeta";
        localVarRequestOptions.OperationIndex = operationIndex;

        // authentication (Bearer) required
        // oauth required
        if (!localVarRequestOptions.HeaderParameters.ContainsKey(BlinkDebitConstant.AUTHORIZATION.GetValue()))
        {
            if (!string.IsNullOrEmpty(Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.AUTHORIZATION.GetValue(),
                    BlinkDebitConstant.BEARER.GetValue() + Configuration.AccessToken);
            }
            else if (!string.IsNullOrEmpty(Configuration.OAuthTokenUrl) &&
                     !string.IsNullOrEmpty(Configuration.OAuthClientId) &&
                     !string.IsNullOrEmpty(Configuration.OAuthClientSecret) &&
                     Configuration.OAuthFlow != null)
            {
                localVarRequestOptions.OAuth = true;
            }
        }

        // make the HTTP request
        var localVarResponse =
            Client.Get<List<BankMetadata>>("/meta", localVarRequestOptions, Configuration);
        var exception = ExceptionFactory("GetMeta", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Get Bank Metadata The available banks, features available for the banks, and relevant fields.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of List&lt;BankMetadata&gt;</returns>
    public async Task<List<BankMetadata>> GetMetaAsync(Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarResponse =
            await GetMetaWithHttpInfoAsync(requestHeaders, operationIndex, cancellationToken)
                .ConfigureAwait(false);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Get Bank Metadata The available banks, features available for the banks, and relevant fields.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (List&lt;BankMetadata&gt;)</returns>
    public async Task<ApiResponse<List<BankMetadata>>>
        GetMetaWithHttpInfoAsync(Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
            CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarRequestOptions = new RequestOptions();

        var contentTypes = Array.Empty<string>();

        // to determine the Accept header
        var accepts = new[]
        {
            "application/json"
        };

        var localVarContentType = ClientUtils.SelectHeaderContentType(contentTypes);
        localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

        var localVarAccept = ClientUtils.SelectHeaderAccept(accepts);
        localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

        requestHeaders ??= new Dictionary<string, string?>();
        if (requestHeaders.ContainsKey(BlinkDebitConstant.REQUEST_ID.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.REQUEST_ID.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.REQUEST_ID.GetValue())));
        }

        if (requestHeaders.ContainsKey(BlinkDebitConstant.CORRELATION_ID.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.CORRELATION_ID.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.CORRELATION_ID.GetValue())));
        }

        if (requestHeaders.ContainsKey(BlinkDebitConstant.CUSTOMER_IP.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.CUSTOMER_IP.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.CUSTOMER_IP.GetValue())));
        }

        if (requestHeaders.ContainsKey(BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue())));
        }

        localVarRequestOptions.Operation = "BankMetadataApi.GetMeta";
        localVarRequestOptions.OperationIndex = operationIndex;

        // authentication (Bearer) required
        // oauth required
        if (!localVarRequestOptions.HeaderParameters.ContainsKey(BlinkDebitConstant.AUTHORIZATION.GetValue()))
        {
            if (!string.IsNullOrEmpty(Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.AUTHORIZATION.GetValue(),
                    BlinkDebitConstant.BEARER.GetValue() + Configuration.AccessToken);
            }
            else if (!string.IsNullOrEmpty(Configuration.OAuthTokenUrl) &&
                     !string.IsNullOrEmpty(Configuration.OAuthClientId) &&
                     !string.IsNullOrEmpty(Configuration.OAuthClientSecret) &&
                     Configuration.OAuthFlow != null)
            {
                localVarRequestOptions.OAuth = true;
            }
        }

        // make the HTTP request
        var localVarResponse = await AsynchronousClient
            .GetAsync<List<BankMetadata>>("/meta", localVarRequestOptions, Configuration, cancellationToken)
            .ConfigureAwait(false);
        var exception = ExceptionFactory("GetMeta", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }
}