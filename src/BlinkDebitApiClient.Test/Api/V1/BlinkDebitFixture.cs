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
using BlinkDebitApiClient.Api.V1;
using BlinkDebitApiClient.Client;
using BlinkDebitApiClient.Client.Auth;
using BlinkDebitApiClient.Config;
using Microsoft.Extensions.Logging;
using Xunit;

namespace BlinkDebitApiClient.Test.Api.V1;

public class BlinkDebitFixture
{
    public BlinkDebitFixture()
    {
        var initialConfiguration = new Configuration
        {
            BasePath = "https://sandbox.debit.blinkpay.co.nz/payments/v1",
            OAuthTokenUrl = "https://sandbox.debit.blinkpay.co.nz/oauth2/token",
            OAuthClientId = Environment.GetEnvironmentVariable("BLINKPAY_CLIENT_ID") ?? string.Empty,
            OAuthClientSecret = Environment.GetEnvironmentVariable("BLINKPAY_CLIENT_SECRET") ?? string.Empty,
            OAuthFlow = OAuthFlow.APPLICATION
        };

        var logger = LoggerFactory
            .Create(builder => builder
                .AddConsole()
                .AddDebug())
            .CreateLogger<BlinkDebitFixture>();

        var finalConfiguration = Configuration.MergeConfigurations(GlobalConfiguration.Instance, initialConfiguration);
        var apiClient = new ApiClient(logger, finalConfiguration);

        BankMetadataApi = new BankMetadataApi(logger, apiClient, apiClient, finalConfiguration);
        SingleConsentsApi = new SingleConsentsApi(logger, apiClient, apiClient, finalConfiguration);
        EnduringConsentsApi = new EnduringConsentsApi(logger, apiClient, apiClient, finalConfiguration);
        QuickPaymentsApi = new QuickPaymentsApi(logger, apiClient, apiClient, finalConfiguration);
        PaymentsApi = new PaymentsApi(logger, apiClient, apiClient, finalConfiguration);
        RefundsApi = new RefundsApi(logger, apiClient, apiClient, finalConfiguration);
        BlinkDebitClient = new BlinkDebitClient(logger, apiClient, finalConfiguration);
    }

    public BankMetadataApi BankMetadataApi { get; }

    public SingleConsentsApi SingleConsentsApi { get; }

    public EnduringConsentsApi EnduringConsentsApi { get; }

    public QuickPaymentsApi QuickPaymentsApi { get; }

    public PaymentsApi PaymentsApi { get; }

    public RefundsApi RefundsApi { get; }

    public BlinkDebitClient BlinkDebitClient { get; }
}

[CollectionDefinition("Blink Debit Collection")]
public class BlinkDebitCollection : ICollectionFixture<BlinkDebitFixture>
{
}