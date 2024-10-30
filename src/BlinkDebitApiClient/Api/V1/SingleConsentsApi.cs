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
public interface ISingleConsentsApiSync : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// Create Single Consent
    /// </summary>
    /// <remarks>
    /// Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>CreateConsentResponse</returns>
    CreateConsentResponse CreateSingleConsent(Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0);

    /// <summary>
    /// Create Single Consent
    /// </summary>
    /// <remarks>
    /// Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of CreateConsentResponse</returns>
    ApiResponse<CreateConsentResponse> CreateSingleConsentWithHttpInfo(
        Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0);

    /// <summary>
    /// Get Single Consent
    /// </summary>
    /// <remarks>
    /// Get an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>Consent</returns>
    Consent GetSingleConsent(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0);

    /// <summary>
    /// Get Single Consent
    /// </summary>
    /// <remarks>
    /// Get an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of Consent</returns>
    ApiResponse<Consent> GetSingleConsentWithHttpInfo(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0);

    /// <summary>
    /// Revoke Single Consent
    /// </summary>
    /// <remarks>
    /// Revoke an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns></returns>
    void RevokeSingleConsent(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0);

    /// <summary>
    /// Revoke Single Consent
    /// </summary>
    /// <remarks>
    /// Revoke an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<Object> RevokeSingleConsentWithHttpInfo(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0);

    #endregion Synchronous Operations
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface ISingleConsentsApiAsync : IApiAccessor
{
    #region Asynchronous Operations

    /// <summary>
    /// Create Single Consent
    /// </summary>
    /// <remarks>
    /// Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CreateConsentResponse</returns>
    Task<CreateConsentResponse> CreateSingleConsentAsync(Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Create Single Consent
    /// </summary>
    /// <remarks>
    /// Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (CreateConsentResponse)</returns>
    Task<ApiResponse<CreateConsentResponse>> CreateSingleConsentWithHttpInfoAsync(
        Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Get Single Consent
    /// </summary>
    /// <remarks>
    /// Get an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of Consent</returns>
    Task<Consent> GetSingleConsentAsync(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Get Single Consent
    /// </summary>
    /// <remarks>
    /// Get an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (Consent)</returns>
    Task<ApiResponse<Consent>> GetSingleConsentWithHttpInfoAsync(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Revoke Single Consent
    /// </summary>
    /// <remarks>
    /// Revoke an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    Task RevokeSingleConsentAsync(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Revoke Single Consent
    /// </summary>
    /// <remarks>
    /// Revoke an existing consent by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<Object>> RevokeSingleConsentWithHttpInfoAsync(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    #endregion Asynchronous Operations
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface ISingleConsentsApi : ISingleConsentsApiSync, ISingleConsentsApiAsync
{
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public class SingleConsentsApi : ISingleConsentsApi
{
    private ExceptionFactory _exceptionFactory = (name, response, logger) => null;

    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleConsentsApi"/> class.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="basePath">The base path containing the Blink Debit API URL and the default path (/payments/v1)</param>
    /// <returns></returns>
    public SingleConsentsApi(ILogger logger, string basePath)
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
    /// Initializes a new instance of the <see cref="SingleConsentsApi"/> class
    /// using Configuration object
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="configuration">An instance of Configuration</param>
    /// <returns></returns>
    public SingleConsentsApi(ILogger logger, Configuration configuration)
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
    /// Initializes a new instance of the <see cref="SingleConsentsApi"/> class
    /// using a Configuration object and client instance.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="client">The client interface for synchronous API access.</param>
    /// <param name="asyncClient">The client interface for asynchronous API access.</param>
    /// <param name="configuration">The configuration object.</param>
    public SingleConsentsApi(ILogger logger, ISynchronousClient client, IAsynchronousClient asyncClient,
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
    /// Create Single Consent Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>CreateConsentResponse</returns>
    public CreateConsentResponse CreateSingleConsent(Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0)
    {
        var localVarResponse = CreateSingleConsentWithHttpInfo(requestHeaders, singleConsentRequest);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Create Single Consent Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of CreateConsentResponse</returns>
    public ApiResponse<CreateConsentResponse> CreateSingleConsentWithHttpInfo(
        Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0)
    {
        var localVarRequestOptions = new RequestOptions();

        var contentTypes = new[]
        {
            "application/json"
        };

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

        if (requestHeaders.ContainsKey(BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue())));
        }

        if (singleConsentRequest != null) localVarRequestOptions.Data = singleConsentRequest;

        localVarRequestOptions.Operation = "SingleConsentsApi.CreateSingleConsent";
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
            Client.Post<CreateConsentResponse>("/single-consents", localVarRequestOptions, Configuration);
        var exception = ExceptionFactory("CreateSingleConsent", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Create Single Consent Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CreateConsentResponse</returns>
    public async Task<CreateConsentResponse> CreateSingleConsentAsync(
        Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarResponse = await CreateSingleConsentWithHttpInfoAsync(requestHeaders, singleConsentRequest,
                operationIndex,
                cancellationToken)
            .ConfigureAwait(false);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Create Single Consent Create a single payment consent request with the bank that will go to the customer for approval.  A successful response does not indicate a completed consent. The status of the consent can be subsequently checked with the consent ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="singleConsentRequest">The consent request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (CreateConsentResponse)</returns>
    public async Task<ApiResponse<CreateConsentResponse>> CreateSingleConsentWithHttpInfoAsync(
        Dictionary<string, string?>? requestHeaders = null,
        SingleConsentRequest? singleConsentRequest = default(SingleConsentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarRequestOptions = new RequestOptions();

        var contentTypes = new[]
        {
            "application/json"
        };

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

        if (requestHeaders.ContainsKey(BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()))
        {
            localVarRequestOptions.HeaderParameters.Add(BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue(),
                ClientUtils.ParameterToString(
                    requestHeaders.GetValueOrDefault(BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue())));
        }

        if (singleConsentRequest != null) localVarRequestOptions.Data = singleConsentRequest;

        localVarRequestOptions.Operation = "SingleConsentsApi.CreateSingleConsent";
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
            .PostAsync<CreateConsentResponse>("/single-consents", localVarRequestOptions, Configuration,
                cancellationToken).ConfigureAwait(false);
        var exception = ExceptionFactory("CreateSingleConsent", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Get Single Consent Get an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>Consent</returns>
    public Consent GetSingleConsent(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0)
    {
        var localVarResponse = GetSingleConsentWithHttpInfo(consentId, requestHeaders);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Get Single Consent Get an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of Consent</returns>
    public ApiResponse<Consent> GetSingleConsentWithHttpInfo(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0)
    {
        var localVarRequestOptions = new RequestOptions();

        var contentTypes = new[]
        {
            "application/json"
        };

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

        localVarRequestOptions.PathParameters.Add("consent_id",
            ClientUtils.ParameterToString(consentId)); // path parameter

        localVarRequestOptions.Operation = "SingleConsentsApi.GetSingleConsent";
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
        var localVarResponse = Client.Get<Consent>("/single-consents/{consent_id}", localVarRequestOptions,
            Configuration);
        var exception = ExceptionFactory("GetSingleConsent", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Get Single Consent Get an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of Consent</returns>
    public async Task<Consent> GetSingleConsentAsync(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarResponse =
            await GetSingleConsentWithHttpInfoAsync(consentId, requestHeaders, operationIndex,
                cancellationToken).ConfigureAwait(false);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Get Single Consent Get an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (Consent)</returns>
    public async Task<ApiResponse<Consent>> GetSingleConsentWithHttpInfoAsync(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
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

        localVarRequestOptions.PathParameters.Add("consent_id",
            ClientUtils.ParameterToString(consentId)); // path parameter

        localVarRequestOptions.Operation = "SingleConsentsApi.GetSingleConsent";
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
        var localVarResponse = await AsynchronousClient.GetAsync<Consent>("/single-consents/{consent_id}",
            localVarRequestOptions, Configuration, cancellationToken).ConfigureAwait(false);
        var exception = ExceptionFactory("GetSingleConsent", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Revoke Single Consent Revoke an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns></returns>
    public void RevokeSingleConsent(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0)
    {
        RevokeSingleConsentWithHttpInfo(consentId, requestHeaders);
    }

    /// <summary>
    /// Revoke Single Consent Revoke an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of Object(void)</returns>
    public ApiResponse<Object> RevokeSingleConsentWithHttpInfo(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0)
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

        localVarRequestOptions.PathParameters.Add("consent_id",
            ClientUtils.ParameterToString(consentId)); // path parameter

        localVarRequestOptions.Operation = "SingleConsentsApi.RevokeSingleConsent";
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
        var localVarResponse = Client.Delete<Object>("/single-consents/{consent_id}", localVarRequestOptions,
            Configuration);
        var exception = ExceptionFactory("RevokeSingleConsent", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Revoke Single Consent Revoke an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    public async Task RevokeSingleConsentAsync(Guid consentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken))
    {
        await RevokeSingleConsentWithHttpInfoAsync(consentId, requestHeaders, operationIndex,
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Revoke Single Consent Revoke an existing consent by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse</returns>
    public async Task<ApiResponse<Object>> RevokeSingleConsentWithHttpInfoAsync(Guid consentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
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

        localVarRequestOptions.PathParameters.Add("consent_id",
            ClientUtils.ParameterToString(consentId)); // path parameter

        localVarRequestOptions.Operation = "SingleConsentsApi.RevokeSingleConsent";
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
        var localVarResponse = await AsynchronousClient.DeleteAsync<Object>("/single-consents/{consent_id}",
            localVarRequestOptions, Configuration, cancellationToken).ConfigureAwait(false);
        var exception = ExceptionFactory("RevokeSingleConsent", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }
}