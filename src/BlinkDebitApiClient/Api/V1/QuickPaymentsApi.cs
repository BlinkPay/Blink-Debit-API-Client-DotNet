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
public interface IQuickPaymentsApiSync : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// Create Quick Payment
    /// </summary>
    /// <remarks>
    /// Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>CreateQuickPaymentResponse</returns>
    CreateQuickPaymentResponse CreateQuickPayment(Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0);

    /// <summary>
    /// Create Quick Payment
    /// </summary>
    /// <remarks>
    /// Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of CreateQuickPaymentResponse</returns>
    ApiResponse<CreateQuickPaymentResponse> CreateQuickPaymentWithHttpInfo(
        Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0);

    /// <summary>
    /// Get Quick Payment
    /// </summary>
    /// <remarks>
    /// Get a quick payment by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>QuickPaymentResponse</returns>
    QuickPaymentResponse GetQuickPayment(Guid quickPaymentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0);

    /// <summary>
    /// Get Quick Payment
    /// </summary>
    /// <remarks>
    /// Get a quick payment by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of QuickPaymentResponse</returns>
    ApiResponse<QuickPaymentResponse> GetQuickPaymentWithHttpInfo(Guid quickPaymentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0);

    /// <summary>
    /// Revoke Quick Payment
    /// </summary>
    /// <remarks>
    /// Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns></returns>
    void RevokeQuickPayment(Guid quickPaymentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0);

    /// <summary>
    /// Revoke Quick Payment
    /// </summary>
    /// <remarks>
    /// Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<Object> RevokeQuickPaymentWithHttpInfo(Guid quickPaymentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0);

    #endregion Synchronous Operations
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IQuickPaymentsApiAsync : IApiAccessor
{
    #region Asynchronous Operations

    /// <summary>
    /// Create Quick Payment
    /// </summary>
    /// <remarks>
    /// Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CreateQuickPaymentResponse</returns>
    Task<CreateQuickPaymentResponse> CreateQuickPaymentAsync(Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Create Quick Payment
    /// </summary>
    /// <remarks>
    /// Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (CreateQuickPaymentResponse)</returns>
    Task<ApiResponse<CreateQuickPaymentResponse>> CreateQuickPaymentWithHttpInfoAsync(
        Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Get Quick Payment
    /// </summary>
    /// <remarks>
    /// Get a quick payment by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of QuickPaymentResponse</returns>
    Task<QuickPaymentResponse> GetQuickPaymentAsync(Guid quickPaymentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Get Quick Payment
    /// </summary>
    /// <remarks>
    /// Get a quick payment by ID.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (QuickPaymentResponse)</returns>
    Task<ApiResponse<QuickPaymentResponse>> GetQuickPaymentWithHttpInfoAsync(Guid quickPaymentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Revoke Quick Payment
    /// </summary>
    /// <remarks>
    /// Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    Task RevokeQuickPaymentAsync(Guid quickPaymentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Revoke Quick Payment
    /// </summary>
    /// <remarks>
    /// Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </remarks>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<Object>> RevokeQuickPaymentWithHttpInfoAsync(Guid quickPaymentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken));

    #endregion Asynchronous Operations
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IQuickPaymentsApi : IQuickPaymentsApiSync, IQuickPaymentsApiAsync
{
}

/// <summary>
/// Represents a collection of functions to interact with the API endpoints
/// </summary>
public class QuickPaymentsApi : IQuickPaymentsApi
{
    private ExceptionFactory _exceptionFactory = (name, response, logger) => null;

    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuickPaymentsApi"/> class.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="basePath">The base path containing the Blink Debit API URL and the default path (/payments/v1)</param>
    /// <returns></returns>
    public QuickPaymentsApi(ILogger logger, string basePath)
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
    /// Initializes a new instance of the <see cref="QuickPaymentsApi"/> class
    /// using Configuration object
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="configuration">An instance of Configuration</param>
    /// <returns></returns>
    public QuickPaymentsApi(ILogger logger, Configuration configuration)
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
    /// Initializes a new instance of the <see cref="QuickPaymentsApi"/> class
    /// using a Configuration object and client instance.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="client">The client interface for synchronous API access.</param>
    /// <param name="asyncClient">The client interface for asynchronous API access.</param>
    /// <param name="configuration">The configuration object.</param>
    public QuickPaymentsApi(ILogger logger, ISynchronousClient client, IAsynchronousClient asyncClient,
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
    /// Create Quick Payment Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>CreateQuickPaymentResponse</returns>
    public CreateQuickPaymentResponse CreateQuickPayment(Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0)
    {
        var localVarResponse = CreateQuickPaymentWithHttpInfo(requestHeaders, quickPaymentRequest);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Create Quick Payment Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of CreateQuickPaymentResponse</returns>
    public ApiResponse<CreateQuickPaymentResponse> CreateQuickPaymentWithHttpInfo(
        Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0)
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

        if (quickPaymentRequest != null) localVarRequestOptions.Data = quickPaymentRequest;

        localVarRequestOptions.Operation = "QuickPaymentsApi.CreateQuickPayment";
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
            Client.Post<CreateQuickPaymentResponse>("/quick-payments", localVarRequestOptions, Configuration);
        var exception = ExceptionFactory("CreateQuickPayment", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Create Quick Payment Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CreateQuickPaymentResponse</returns>
    public async Task<CreateQuickPaymentResponse> CreateQuickPaymentAsync(
        Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarResponse =
            await CreateQuickPaymentWithHttpInfoAsync(requestHeaders, quickPaymentRequest, operationIndex,
                cancellationToken).ConfigureAwait(false);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Create Quick Payment Create a quick payment, which both obtains the consent and debits the requested one-off payment.  This endpoint begins the customer consent process. Once the consent is authorised, Blink automatically attempts to debit the payment.  A successful response does **not** indicate a successful debit. The payment status can be checked by subsequent calls to the single payment endpoint.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="quickPaymentRequest">The single payment request parameters. (optional)</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (CreateQuickPaymentResponse)</returns>
    public async Task<ApiResponse<CreateQuickPaymentResponse>> CreateQuickPaymentWithHttpInfoAsync(
        Dictionary<string, string?>? requestHeaders = null,
        QuickPaymentRequest? quickPaymentRequest = default(QuickPaymentRequest?), int operationIndex = 0,
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

        if (quickPaymentRequest != null) localVarRequestOptions.Data = quickPaymentRequest;

        localVarRequestOptions.Operation = "QuickPaymentsApi.CreateQuickPayment";
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
            .PostAsync<CreateQuickPaymentResponse>("/quick-payments", localVarRequestOptions, Configuration,
                cancellationToken).ConfigureAwait(false);
        var exception = ExceptionFactory("CreateQuickPayment", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Get Quick Payment Get a quick payment by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>QuickPaymentResponse</returns>
    public QuickPaymentResponse GetQuickPayment(Guid quickPaymentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0)
    {
        var localVarResponse = GetQuickPaymentWithHttpInfo(quickPaymentId, requestHeaders);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Get Quick Payment Get a quick payment by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of QuickPaymentResponse</returns>
    public ApiResponse<QuickPaymentResponse> GetQuickPaymentWithHttpInfo(Guid quickPaymentId,
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

        localVarRequestOptions.PathParameters.Add("quick_payment_id",
            ClientUtils.ParameterToString(quickPaymentId)); // path parameter

        localVarRequestOptions.Operation = "QuickPaymentsApi.GetQuickPayment";
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
        var localVarResponse = Client.Get<QuickPaymentResponse>("/quick-payments/{quick_payment_id}",
            localVarRequestOptions, Configuration);
        var exception = ExceptionFactory("GetQuickPayment", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Get Quick Payment Get a quick payment by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of QuickPaymentResponse</returns>
    public async Task<QuickPaymentResponse> GetQuickPaymentAsync(Guid quickPaymentId,
        Dictionary<string, string?>? requestHeaders = null, int operationIndex = 0,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var localVarResponse =
            await GetQuickPaymentWithHttpInfoAsync(quickPaymentId, requestHeaders, operationIndex,
                cancellationToken).ConfigureAwait(false);
        return localVarResponse.Data;
    }

    /// <summary>
    /// Get Quick Payment Get a quick payment by ID.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse (QuickPaymentResponse)</returns>
    public async Task<ApiResponse<QuickPaymentResponse>> GetQuickPaymentWithHttpInfoAsync(Guid quickPaymentId,
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

        localVarRequestOptions.PathParameters.Add("quick_payment_id",
            ClientUtils.ParameterToString(quickPaymentId)); // path parameter

        localVarRequestOptions.Operation = "QuickPaymentsApi.GetQuickPayment";
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
            .GetAsync<QuickPaymentResponse>("/quick-payments/{quick_payment_id}", localVarRequestOptions,
                Configuration, cancellationToken).ConfigureAwait(false);
        var exception = ExceptionFactory("GetQuickPayment", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Revoke Quick Payment Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns></returns>
    public void RevokeQuickPayment(Guid quickPaymentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0)
    {
        RevokeQuickPaymentWithHttpInfo(quickPaymentId, requestHeaders);
    }

    /// <summary>
    /// Revoke Quick Payment Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <returns>ApiResponse of Object(void)</returns>
    public ApiResponse<Object> RevokeQuickPaymentWithHttpInfo(Guid quickPaymentId,
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

        localVarRequestOptions.PathParameters.Add("quick_payment_id",
            ClientUtils.ParameterToString(quickPaymentId)); // path parameter

        localVarRequestOptions.Operation = "QuickPaymentsApi.RevokeQuickPayment";
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
        var localVarResponse = Client.Delete<Object>("/quick-payments/{quick_payment_id}", localVarRequestOptions,
            Configuration);
        var exception = ExceptionFactory("RevokeQuickPayment", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }

    /// <summary>
    /// Revoke Quick Payment Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    public async Task RevokeQuickPaymentAsync(Guid quickPaymentId, Dictionary<string, string?>? requestHeaders = null,
        int operationIndex = 0, CancellationToken cancellationToken = default(CancellationToken))
    {
        await RevokeQuickPaymentWithHttpInfoAsync(quickPaymentId, requestHeaders, operationIndex,
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Revoke Quick Payment Revoke an existing (unpaid) quick payment by ID.  The quick payment cannot be revoked if the payment has already been made.
    /// </summary>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestHeaders">the Dictionary of optional request headers.</param>
    /// <param name="operationIndex">Index associated with the operation.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ApiResponse</returns>
    public async Task<ApiResponse<Object>> RevokeQuickPaymentWithHttpInfoAsync(Guid quickPaymentId,
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

        localVarRequestOptions.PathParameters.Add("quick_payment_id",
            ClientUtils.ParameterToString(quickPaymentId)); // path parameter

        localVarRequestOptions.Operation = "QuickPaymentsApi.RevokeQuickPayment";
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
        var localVarResponse = await AsynchronousClient.DeleteAsync<Object>("/quick-payments/{quick_payment_id}",
            localVarRequestOptions, Configuration, cancellationToken).ConfigureAwait(false);
        var exception = ExceptionFactory("RevokeQuickPayment", localVarResponse, _logger);
        if (exception != null)
        {
            throw exception;
        }

        return localVarResponse;
    }
}