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
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using BlinkDebitApiClient.Client;
using BlinkDebitApiClient.Client.Auth;
using BlinkDebitApiClient.Config;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RestSharp;

namespace BlinkDebitApiClient.Api.V1;

/// <summary>
/// The facade for accessing all client methods from one place.
/// </summary>
public class BlinkDebitClient
{
    private readonly ILogger _logger;

    private readonly SingleConsentsApi _singleConsentsApi;

    private readonly EnduringConsentsApi _enduringConsentsApi;

    private readonly QuickPaymentsApi _quickPaymentsApi;

    private readonly PaymentsApi _paymentsApi;

    private readonly RefundsApi _refundsApi;

    private readonly BankMetadataApi _bankMetadataApi;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="singleConsentsApi"></param>
    /// <param name="enduringConsentsApi"></param>
    /// <param name="quickPaymentsApi"></param>
    /// <param name="paymentsApi"></param>
    /// <param name="refundsApi"></param>
    /// <param name="bankMetadataApi"></param>
    public BlinkDebitClient(ILogger logger, SingleConsentsApi singleConsentsApi,
        EnduringConsentsApi enduringConsentsApi, QuickPaymentsApi quickPaymentsApi, PaymentsApi paymentsApi,
        RefundsApi refundsApi, BankMetadataApi bankMetadataApi)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _singleConsentsApi = singleConsentsApi;
        _enduringConsentsApi = enduringConsentsApi;
        _quickPaymentsApi = quickPaymentsApi;
        _paymentsApi = paymentsApi;
        _refundsApi = refundsApi;
        _bankMetadataApi = bankMetadataApi;
    }

    /// <summary>
    /// Constructor with ILogger
    /// </summary>
    /// <param name="logger">The logger</param>
    public BlinkDebitClient(ILogger logger) : this(logger, GenerateBlinkPayProperties())
    {
    }

    /// <summary>
    /// Constructor with ILogger and BlinkPayProperties
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="blinkPayProperties">The BlinkPayProperties</param>
    public BlinkDebitClient(ILogger logger, BlinkPayProperties blinkPayProperties) : this(logger,
        Environment.GetEnvironmentVariable("BLINKPAY_DEBIT_URL") ?? blinkPayProperties.DebitUrl,
        Environment.GetEnvironmentVariable("BLINKPAY_CLIENT_ID") ?? blinkPayProperties.ClientId,
        Environment.GetEnvironmentVariable("BLINKPAY_CLIENT_SECRET") ?? blinkPayProperties.ClientSecret,
        bool.TryParse(Environment.GetEnvironmentVariable("BLINKPAY_RETRY_ENABLED"), out var retryEnabled)
            ? retryEnabled
            : blinkPayProperties.RetryEnabled
    )
    {
    }

    /// <summary>
    /// The constructor with the essential parameters
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="debitUrl">The Blink Debit URL</param>
    /// <param name="clientId">The OAuth2 client ID</param>
    /// <param name="clientSecret">The OAuth2 client secret</param>
    /// <param name="retryEnabled">The flag if retry is enabled</param>
    public BlinkDebitClient(ILogger logger, string debitUrl, string clientId, string clientSecret,
        bool? retryEnabled = true)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var initialConfiguration = new Configuration
        {
            BasePath = debitUrl + "/payments/v1",
            Timeout = 10000,
            OAuthTokenUrl = debitUrl + "/oauth2/token",
            OAuthClientId = clientId,
            OAuthClientSecret = clientSecret,
            OAuthFlow = OAuthFlow.APPLICATION,
            RetryEnabled = retryEnabled != null && retryEnabled.Value
        };
        
        ConfigureRetry(retryEnabled != null && retryEnabled.Value);

        var finalConfiguration = Configuration.MergeConfigurations(GlobalConfiguration.Instance, initialConfiguration);
        var apiClient = new ApiClient(logger, finalConfiguration);

        _bankMetadataApi = new BankMetadataApi(logger, apiClient, apiClient, finalConfiguration);
        _singleConsentsApi = new SingleConsentsApi(logger, apiClient, apiClient, finalConfiguration);
        _enduringConsentsApi = new EnduringConsentsApi(logger, apiClient, apiClient, finalConfiguration);
        _quickPaymentsApi = new QuickPaymentsApi(logger, apiClient, apiClient, finalConfiguration);
        _paymentsApi = new PaymentsApi(logger, apiClient, apiClient, finalConfiguration);
        _refundsApi = new RefundsApi(logger, apiClient, apiClient, finalConfiguration);
    }

    /// <summary>
    /// Constructor with an existing API client and configuration. Retry policy, if needed, must also be configured
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="apiClient">The API client</param>
    /// <param name="configuration">The configuration</param>
    public BlinkDebitClient(ILogger logger, ApiClient apiClient, IReadableConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _bankMetadataApi = new BankMetadataApi(logger, apiClient, apiClient, configuration);
        _singleConsentsApi = new SingleConsentsApi(logger, apiClient, apiClient, configuration);
        _enduringConsentsApi = new EnduringConsentsApi(logger, apiClient, apiClient, configuration);
        _quickPaymentsApi = new QuickPaymentsApi(logger, apiClient, apiClient, configuration);
        _paymentsApi = new PaymentsApi(logger, apiClient, apiClient, configuration);
        _refundsApi = new RefundsApi(logger, apiClient, apiClient, configuration);
    }

    private static BlinkPayProperties GenerateBlinkPayProperties()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var blinkPayProperties = new BlinkPayProperties();
        config.GetSection("BlinkPay").Bind(blinkPayProperties);

        return blinkPayProperties;
    }

    private static void ConfigureRetry(bool retryEnabled)
    {
        if (!retryEnabled)
        {
            return;
        }

        var random = new Random();

        var policyBuilder = Policy<RestResponse>
            .Handle<BlinkRequestTimeoutException>()
            .Or<BlinkServiceException>()
            .Or<SocketException>()
            .Or<WebException>()
            .Or<HttpRequestException>();

        RetryConfiguration.RetryPolicy = policyBuilder
            .WaitAndRetry(3, // Number of retries
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential back-off: 2, 4, 8 etc
                                + TimeSpan.FromMilliseconds(random.Next(0, 1000)) // plus some milliseconds random delay
            );

        RetryConfiguration.AsyncRetryPolicy = policyBuilder
            .WaitAndRetryAsync(3, // Number of retries
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential back-off: 2, 4, 8 etc
                                + TimeSpan.FromMilliseconds(random.Next(0, 1000)) // plus some milliseconds random delay
            );
    }

    /// <summary>
    /// Returns the BankMetadata List
    /// </summary>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit.</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit.</param>
    /// <returns>Returns BankMetadata List</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public List<BankMetadata> GetMeta(Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return GetMetaAsync(requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Returns the BankMetadata List
    /// </summary>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit.</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit.</param>
    /// <returns>Returns Task of BankMetadata List</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<List<BankMetadata>> GetMetaAsync(Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _bankMetadataApi.GetMetaAsync(requestId, xCorrelationId);
    }

    /// <summary>
    /// Creates a single consent
    /// </summary>
    /// <param name="singleConsentRequest">The consent request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="xCustomerIp">The customers IP address (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>CreateConsentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public CreateConsentResponse CreateSingleConsent(SingleConsentRequest singleConsentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), string? xCustomerIp = default(string?),
        Guid? idempotencyKey = default(Guid?))
    {
        try
        {
            return CreateSingleConsentAsync(singleConsentRequest, requestId, xCorrelationId, xCustomerIp,
                idempotencyKey).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates a single consent
    /// </summary>
    /// <param name="singleConsentRequest">The consent request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="xCustomerIp">The customers IP address (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>Task of CreateConsentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<CreateConsentResponse> CreateSingleConsentAsync(SingleConsentRequest? singleConsentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), string? xCustomerIp = default(string?),
        Guid? idempotencyKey = default(Guid?))
    {
        return await _singleConsentsApi.CreateSingleConsentAsync(requestId, xCorrelationId, xCustomerIp, idempotencyKey,
            singleConsentRequest);
    }

    /// <summary>
    /// Retrieves an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Consent</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public Consent GetSingleConsent(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return GetSingleConsentAsync(consentId, requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of Consent</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<Consent> GetSingleConsentAsync(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _singleConsentsApi.GetSingleConsentAsync(consentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Retrieves an authorised single consent by ID within the specified time
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Consent</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public Consent AwaitAuthorisedSingleConsent(Guid consentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<Consent>
            .Handle<BlinkConsentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        throw new BlinkConsentTimeoutException();
                    }
                });

        try
        {
            return retryPolicy.ExecuteAsync(async () =>
            {
                var consent = await GetSingleConsentAsync(consentId);

                var status = consent.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Single Consent ID: {consentId}", status,
                    consentId);

                if (Consent.StatusEnum.Authorised == status)
                {
                    return consent;
                }

                throw new BlinkConsentFailureException();
            }).Result;
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an authorised single consent by ID within the specified time
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Consent</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public Consent AwaitAuthorisedSingleConsentOrThrowException(Guid consentId, int maxWaitSeconds)
    {
        try
        {
            return AwaitAuthorisedSingleConsentAsync(consentId, maxWaitSeconds).Result;
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkConsentTimeoutException)
        {
            throw e.InnerException;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an authorised single consent by ID within the specified time. The consent statuses are handled accordingly.
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Task of Consent</returns>
    /// <exception cref="BlinkConsentRejectedException"></exception>
    /// <exception cref="BlinkConsentTimeoutException"></exception>
    /// <exception cref="BlinkConsentFailureException"></exception>
    public async Task<Consent> AwaitAuthorisedSingleConsentAsync(Guid consentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<Consent>
            .Handle<BlinkConsentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        throw new BlinkConsentTimeoutException();
                    }
                });

        try
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var consent = await GetSingleConsentAsync(consentId);

                var status = consent.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Single Consent ID: {consentId}", status,
                    consentId);

                switch (status)
                {
                    case Consent.StatusEnum.Authorised:
                    case Consent.StatusEnum.Consumed:
                        _logger.LogDebug("Single consent completed for ID: {consentId}", consentId);
                        return consent;
                    case Consent.StatusEnum.Rejected:
                    case Consent.StatusEnum.Revoked:
                        throw new BlinkConsentRejectedException(
                            $"Single consent [{consentId}] has been rejected or revoked");
                    case Consent.StatusEnum.GatewayTimeout:
                        throw new BlinkConsentTimeoutException($"Gateway timed out for single consent [{consentId}]");
                    case Consent.StatusEnum.GatewayAwaitingSubmission:
                    case Consent.StatusEnum.AwaitingAuthorisation:
                    default:
                        throw new BlinkConsentFailureException(
                            $"Single consent [{consentId}] is waiting for authorisation");
                }
            });
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message, e);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Revokes an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of void</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public void RevokeSingleConsent(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        RevokeSingleConsentAsync(consentId, requestId, xCorrelationId).Wait();
    }

    /// <summary>
    /// Revokes an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of void</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task RevokeSingleConsentAsync(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        await _singleConsentsApi.RevokeSingleConsentAsync(consentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Creates an enduring consent
    /// </summary>
    /// <param name="enduringConsentRequest">The consent request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="xCustomerIp">The customers IP address (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>CreateConsentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public CreateConsentResponse CreateEnduringConsent(EnduringConsentRequest enduringConsentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), string? xCustomerIp = default(string?),
        Guid? idempotencyKey = default(Guid?))
    {
        try
        {
            return CreateEnduringConsentAsync(enduringConsentRequest, requestId, xCorrelationId, xCustomerIp,
                idempotencyKey).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates an enduring consent
    /// </summary>
    /// <param name="enduringConsentRequest">The consent request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="xCustomerIp">The customers IP address (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>Task of CreateConsentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<CreateConsentResponse> CreateEnduringConsentAsync(EnduringConsentRequest? enduringConsentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), string? xCustomerIp = default(string?),
        Guid? idempotencyKey = default(Guid?))
    {
        return await _enduringConsentsApi.CreateEnduringConsentAsync(requestId, xCorrelationId, xCustomerIp,
            idempotencyKey, enduringConsentRequest);
    }

    /// <summary>
    /// Retrieves an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Consent</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public Consent GetEnduringConsent(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return GetEnduringConsentAsync(consentId, requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of Consent</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<Consent> GetEnduringConsentAsync(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _enduringConsentsApi.GetEnduringConsentAsync(consentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Retrieves an authorised enduring consent by ID within the specified time
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Consent</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public Consent AwaitAuthorisedEnduringConsent(Guid consentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<Consent>
            .Handle<BlinkConsentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        var blinkConsentTimeoutException = new BlinkConsentTimeoutException();

                        try
                        {
                            RevokeEnduringConsentAsync(consentId).Wait();
                            _logger.LogInformation(
                                "The max wait time was reached while waiting for the enduring consent to complete and the payment has been revoked with the server. Enduring consent ID: {consentId}",
                                consentId);
                        }
                        catch (Exception revokeException)
                        {
                            _logger.LogError(
                                "Waiting for the enduring consent was not successful and it was also not able to be revoked with the server due to: {message}. Enduring consent ID: {consentId}",
                                revokeException.Message, consentId);
                            throw new AggregateException(blinkConsentTimeoutException, revokeException);
                        }

                        throw blinkConsentTimeoutException;
                    }
                });

        try
        {
            return retryPolicy.ExecuteAsync(async () =>
            {
                var consent = await GetEnduringConsentAsync(consentId);

                var status = consent.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Enduring Consent ID: {consentId}", status,
                    consentId);

                if (Consent.StatusEnum.Authorised == status)
                {
                    return consent;
                }

                throw new BlinkConsentFailureException();
            }).Result;
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an authorised enduring consent by ID within the specified time
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Consent</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public Consent AwaitAuthorisedEnduringConsentOrThrowException(Guid consentId, int maxWaitSeconds)
    {
        try
        {
            return AwaitAuthorisedEnduringConsentAsync(consentId, maxWaitSeconds).Result;
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkConsentTimeoutException)
        {
            throw e.InnerException;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an authorised enduring consent by ID within the specified time. The consent statuses are handled accordingly.
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Task of Consent</returns>
    /// <exception cref="BlinkConsentRejectedException"></exception>
    /// <exception cref="BlinkConsentTimeoutException"></exception>
    /// <exception cref="BlinkConsentFailureException"></exception>
    public async Task<Consent> AwaitAuthorisedEnduringConsentAsync(Guid consentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<Consent>
            .Handle<BlinkConsentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        var blinkConsentTimeoutException = new BlinkConsentTimeoutException();

                        try
                        {
                            RevokeEnduringConsentAsync(consentId).Wait();
                            _logger.LogInformation(
                                "The max wait time was reached while waiting for the enduring consent to complete and the payment has been revoked with the server. Enduring consent ID: {consentId}",
                                consentId);
                        }
                        catch (Exception revokeException)
                        {
                            _logger.LogError(
                                "Waiting for the enduring consent was not successful and it was also not able to be revoked with the server due to: {message}. Enduring consent ID: {consentId}",
                                revokeException.Message, consentId);
                            throw new AggregateException(blinkConsentTimeoutException, revokeException);
                        }

                        throw blinkConsentTimeoutException;
                    }
                });

        try
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var consent = await GetEnduringConsentAsync(consentId);

                var status = consent.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Enduring Consent ID: {consentId}", status,
                    consentId);

                switch (status)
                {
                    case Consent.StatusEnum.Authorised:
                    case Consent.StatusEnum.Consumed:
                        _logger.LogDebug("Enduring consent completed for ID: {consentId}", consentId);
                        return consent;
                    case Consent.StatusEnum.Rejected:
                    case Consent.StatusEnum.Revoked:
                        throw new BlinkConsentRejectedException(
                            $"Enduring consent [{consentId}] has been rejected or revoked");
                    case Consent.StatusEnum.GatewayTimeout:
                        throw new BlinkConsentTimeoutException($"Gateway timed out for enduring consent [{consentId}]");
                    case Consent.StatusEnum.GatewayAwaitingSubmission:
                    case Consent.StatusEnum.AwaitingAuthorisation:
                    default:
                        throw new BlinkConsentFailureException(
                            $"Enduring consent [{consentId}] is waiting for authorisation");
                }
            });
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message, e);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Revokes an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of void</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public void RevokeEnduringConsent(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        RevokeEnduringConsentAsync(consentId, requestId, xCorrelationId).Wait();
    }

    /// <summary>
    /// Revokes an existing consent by ID
    /// </summary>
    /// <param name="consentId">The consent ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of void</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task RevokeEnduringConsentAsync(Guid consentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        await _enduringConsentsApi.RevokeEnduringConsentAsync(consentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Creates a quick payment
    /// </summary>
    /// <param name="quickPaymentRequest">The quick payment request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="xCustomerIp">The customers IP address (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>CreateQuickPaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public CreateQuickPaymentResponse CreateQuickPayment(QuickPaymentRequest quickPaymentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), string? xCustomerIp = default(string?),
        Guid? idempotencyKey = default(Guid?))
    {
        try
        {
            return CreateQuickPaymentAsync(quickPaymentRequest, requestId, xCorrelationId, xCustomerIp,
                idempotencyKey).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates a quick payment
    /// </summary>
    /// <param name="quickPaymentRequest">The quick payment request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="xCustomerIp">The customers IP address (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>Task of CreateQuickPaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<CreateQuickPaymentResponse> CreateQuickPaymentAsync(QuickPaymentRequest? quickPaymentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), string? xCustomerIp = default(string?),
        Guid? idempotencyKey = default(Guid?))
    {
        return await _quickPaymentsApi.CreateQuickPaymentAsync(requestId, xCorrelationId, xCustomerIp, idempotencyKey,
            quickPaymentRequest);
    }

    /// <summary>
    /// Retrieves an existing quick payment by ID
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>QuickPaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public QuickPaymentResponse GetQuickPayment(Guid quickPaymentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return GetQuickPaymentAsync(quickPaymentId, requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an existing quick payment by ID
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of QuickPaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<QuickPaymentResponse> GetQuickPaymentAsync(Guid quickPaymentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _quickPaymentsApi.GetQuickPaymentAsync(quickPaymentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Retrieves a successful quick payment by ID within the specified time
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>QuickPaymentResponse</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public QuickPaymentResponse AwaitSuccessfulQuickPayment(Guid quickPaymentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<QuickPaymentResponse>
            .Handle<BlinkConsentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        var blinkConsentTimeoutException = new BlinkConsentTimeoutException();

                        try
                        {
                            RevokeQuickPaymentAsync(quickPaymentId).Wait();
                            _logger.LogInformation(
                                "The max wait time was reached while waiting for the quick payment to complete and the payment has been revoked with the server. Quick payment ID: {consentId}",
                                quickPaymentId);
                        }
                        catch (Exception revokeException)
                        {
                            _logger.LogError(
                                "Waiting for the quick payment was not successful and it was also not able to be revoked with the server due to: {message}. Quick payment ID: {consentId}",
                                revokeException.Message, quickPaymentId);
                            throw new AggregateException(blinkConsentTimeoutException, revokeException);
                        }

                        throw blinkConsentTimeoutException;
                    }
                });

        try
        {
            return retryPolicy.ExecuteAsync(async () =>
            {
                var quickPayment = await GetQuickPaymentAsync(quickPaymentId);

                var status = quickPayment.Consent.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Quick Payment ID: {consentId}", status,
                    quickPaymentId);

                if (Consent.StatusEnum.Authorised == status)
                {
                    return quickPayment;
                }

                throw new BlinkConsentFailureException();
            }).Result;
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves a successful quick payment by ID within the specified time
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>QuickPaymentResponse</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public QuickPaymentResponse AwaitSuccessfulQuickPaymentOrThrowException(Guid quickPaymentId, int maxWaitSeconds)
    {
        try
        {
            return AwaitSuccessfulQuickPaymentAsync(quickPaymentId, maxWaitSeconds).Result;
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkConsentTimeoutException)
        {
            throw e.InnerException;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves a successful quick payment by ID within the specified time. The consent statuses are handled accordingly.
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Task of QuickPaymentResponse</returns>
    /// <exception cref="BlinkConsentRejectedException"></exception>
    /// <exception cref="BlinkConsentTimeoutException"></exception>
    /// <exception cref="BlinkConsentFailureException"></exception>
    public async Task<QuickPaymentResponse> AwaitSuccessfulQuickPaymentAsync(Guid quickPaymentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<QuickPaymentResponse>
            .Handle<BlinkConsentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        var blinkConsentTimeoutException = new BlinkConsentTimeoutException();

                        try
                        {
                            RevokeQuickPaymentAsync(quickPaymentId).Wait();
                            _logger.LogInformation(
                                "The max wait time was reached while waiting for the quick payment to complete and the payment has been revoked with the server. Quick payment ID: {consentId}",
                                quickPaymentId);
                        }
                        catch (Exception revokeException)
                        {
                            _logger.LogError(
                                "Waiting for the quick payment was not successful and it was also not able to be revoked with the server due to: {message}. Quick payment ID: {consentId}",
                                revokeException.Message, quickPaymentId);
                            throw new AggregateException(blinkConsentTimeoutException, revokeException);
                        }

                        throw blinkConsentTimeoutException;
                    }
                });

        try
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var quickPayment = await GetQuickPaymentAsync(quickPaymentId);

                var status = quickPayment.Consent.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Quick Payment ID: {consentId}", status,
                    quickPaymentId);

                switch (status)
                {
                    case Consent.StatusEnum.Authorised:
                    case Consent.StatusEnum.Consumed:
                        _logger.LogDebug("Quick payment completed for ID: {consentId}", quickPaymentId);
                        return quickPayment;
                    case Consent.StatusEnum.Rejected:
                    case Consent.StatusEnum.Revoked:
                        throw new BlinkConsentRejectedException(
                            $"Quick payment [{quickPaymentId}] has been rejected or revoked");
                    case Consent.StatusEnum.GatewayTimeout:
                        throw new BlinkConsentTimeoutException(
                            $"Gateway timed out for quick payment [{quickPaymentId}]");
                    case Consent.StatusEnum.GatewayAwaitingSubmission:
                    case Consent.StatusEnum.AwaitingAuthorisation:
                    default:
                        throw new BlinkConsentFailureException(
                            $"Quick payment [{quickPaymentId}] is waiting for authorisation");
                }
            });
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message, e);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Revokes an existing quick payment by ID
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of void</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public void RevokeQuickPayment(Guid quickPaymentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        RevokeQuickPaymentAsync(quickPaymentId, requestId, xCorrelationId).Wait();
    }

    /// <summary>
    /// Revokes an existing quick payment by ID
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of void</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task RevokeQuickPaymentAsync(Guid quickPaymentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        await _quickPaymentsApi.RevokeQuickPaymentAsync(quickPaymentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Creates a payment
    /// </summary>
    /// <param name="paymentRequest">The payment request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>PaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public PaymentResponse CreatePayment(PaymentRequest paymentRequest, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?), Guid? idempotencyKey = default(Guid?))
    {
        try
        {
            return CreatePaymentAsync(paymentRequest, requestId, xCorrelationId, idempotencyKey).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates a payment
    /// </summary>
    /// <param name="paymentRequest">The payment request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>Task of PaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest? paymentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), Guid? idempotencyKey = default(Guid?))
    {
        return await _paymentsApi.CreatePaymentAsync(requestId, xCorrelationId, idempotencyKey, paymentRequest);
    }

    /// <summary>
    /// Creates a Westpac payment. Once Westpac enables their Open Banking API, this can be replaced with CreatePayment.
    /// </summary>
    /// <param name="paymentRequest">The payment request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>PaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public PaymentResponse CreateWestpacPayment(PaymentRequest paymentRequest, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?), Guid? idempotencyKey = default(Guid?))
    {
        try
        {
            return CreateWestpacPaymentAsync(paymentRequest, requestId, xCorrelationId, idempotencyKey).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates a Westpac payment. Once Westpac enables their Open Banking API, this can be replaced with CreatePaymentAsync.
    /// </summary>
    /// <param name="paymentRequest">The payment request parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <param name="idempotencyKey">An optional idempotency key to prevent duplicate submissions. (optional)</param>
    /// <returns>Task of PaymentResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<PaymentResponse> CreateWestpacPaymentAsync(PaymentRequest? paymentRequest,
        Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?), Guid? idempotencyKey = default(Guid?))
    {
        return await _paymentsApi.CreatePaymentAsync(requestId, xCorrelationId, idempotencyKey, paymentRequest);
    }

    /// <summary>
    /// Retrieves an existing payment by ID
    /// </summary>
    /// <param name="paymentId">The payment ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Payment</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public Payment GetPayment(Guid paymentId, Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return GetPaymentAsync(paymentId, requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an existing payment by ID
    /// </summary>
    /// <param name="paymentId">The payment ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of Payment</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<Payment> GetPaymentAsync(Guid paymentId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _paymentsApi.GetPaymentAsync(paymentId, requestId, xCorrelationId);
    }

    /// <summary>
    /// Retrieves a successful payment by ID within the specified time
    /// </summary>
    /// <param name="paymentId">The payment ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Payment</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public Payment AwaitSuccessfulPayment(Guid paymentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<Payment>
            .Handle<BlinkPaymentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        throw new BlinkPaymentTimeoutException();
                    }
                });

        try
        {
            return retryPolicy.ExecuteAsync(async () =>
            {
                var payment = await GetPaymentAsync(paymentId);

                var status = payment.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Quick Payment ID: {consentId}", status,
                    paymentId);

                if (Payment.StatusEnum.AcceptedSettlementCompleted == status)
                {
                    return payment;
                }

                throw new BlinkPaymentFailureException();
            }).Result;
        }
        catch (BlinkPaymentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves a successful payment by ID within the specified time
    /// </summary>
    /// <param name="paymentId">The payment ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Payment</returns>
    /// <exception cref="BlinkConsentFailureException">Thrown when a consent exception occurs</exception>
    /// <exception cref="BlinkServiceException">Thrown when a Blink Debit service exception occurs</exception>
    public Payment AwaitSuccessfulPaymentOrThrowException(Guid paymentId, int maxWaitSeconds)
    {
        try
        {
            return AwaitSuccessfulPaymentAsync(paymentId, maxWaitSeconds).Result;
        }
        catch (BlinkPaymentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkPaymentTimeoutException)
        {
            throw e.InnerException;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves a successful payment by ID within the specified time. The payment statuses are handled accordingly.
    /// </summary>
    /// <param name="paymentId">The payment ID</param>
    /// <param name="maxWaitSeconds">The number of seconds to wait</param>
    /// <returns>Task of Payment</returns>
    /// <exception cref="BlinkConsentRejectedException"></exception>
    /// <exception cref="BlinkConsentTimeoutException"></exception>
    /// <exception cref="BlinkConsentFailureException"></exception>
    public async Task<Payment> AwaitSuccessfulPaymentAsync(Guid paymentId, int maxWaitSeconds)
    {
        var retryPolicy = Policy<Payment>
            .Handle<BlinkPaymentFailureException>()
            .WaitAndRetryAsync(maxWaitSeconds, retryAttempt => TimeSpan.FromSeconds(1),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (retryCount == maxWaitSeconds)
                    {
                        throw new BlinkPaymentTimeoutException();
                    }
                });

        try
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var payment = await GetPaymentAsync(paymentId);

                var status = payment.Status;
                _logger.LogDebug("The last status polled was: {status} \tfor Payment ID: {consentId}", status,
                    paymentId);

                switch (status)
                {
                    case Payment.StatusEnum.AcceptedSettlementCompleted:
                        _logger.LogDebug("Payment completed for ID: {consentId}", paymentId);
                        return payment;
                    case Payment.StatusEnum.Rejected:
                        throw new BlinkPaymentRejectedException($"Payment [{paymentId}] has been rejected");
                    case Payment.StatusEnum.AcceptedSettlementInProcess:
                    case Payment.StatusEnum.Pending:
                    default:
                        throw new BlinkPaymentFailureException($"Payment [{paymentId}] is pending or being processed");
                }
            });
        }
        catch (BlinkConsentTimeoutException)
        {
            throw;
        }
        catch (BlinkServiceException)
        {
            throw;
        }
        catch (Exception e) when (e.InnerException is BlinkServiceException)
        {
            throw e.InnerException;
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                throw new BlinkServiceException(e.InnerException.Message, e);
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates a refund
    /// </summary>
    /// <param name="refundDetail">The refund detail parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>RefundResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public RefundResponse CreateRefund(RefundDetail refundDetail, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return CreateRefundAsync(refundDetail, requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Creates a refund
    /// </summary>
    /// <param name="refundDetail">The refund detail parameters</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of RefundResponse</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<RefundResponse> CreateRefundAsync(RefundDetail refundDetail, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _refundsApi.CreateRefundAsync(requestId, xCorrelationId, refundDetail);
    }

    /// <summary>
    /// Retrieves an existing refund by ID
    /// </summary>
    /// <param name="refundId">The refund ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Refund</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public Refund GetRefund(Guid refundId, Guid? requestId = default(Guid?), Guid? xCorrelationId = default(Guid?))
    {
        try
        {
            return GetRefundAsync(refundId, requestId, xCorrelationId).Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is BlinkServiceException blinkServiceException)
            {
                throw blinkServiceException;
            }

            throw new BlinkServiceException(e.Message, e);
        }
    }

    /// <summary>
    /// Retrieves an existing refund by ID
    /// </summary>
    /// <param name="refundId">The refund ID</param>
    /// <param name="requestId">An optional request ID. If provided, it overrides the interaction ID generated by Blink Debit. (optional)</param>
    /// <param name="xCorrelationId">An optional correlation ID for logging chain of events. If provided, it overrides the correlation ID generated by Blink Debit. (optional)</param>
    /// <returns>Task of Refund</returns>
    /// <exception cref="BlinkServiceException">Thrown when an exception occurs</exception>
    public async Task<Refund> GetRefundAsync(Guid refundId, Guid? requestId = default(Guid?),
        Guid? xCorrelationId = default(Guid?))
    {
        return await _refundsApi.GetRefundAsync(refundId, requestId, xCorrelationId);
    }
}