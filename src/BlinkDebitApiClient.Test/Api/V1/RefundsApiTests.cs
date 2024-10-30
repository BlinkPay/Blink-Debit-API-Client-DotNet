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
using System.Threading.Tasks;
using BlinkDebitApiClient.Api.V1;
using BlinkDebitApiClient.Enums;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Xunit;
using Xunit.Abstractions;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
///  Class for testing RefundsApi
/// </summary>
[Collection("Blink Debit Collection")]
public class RefundsApiTests : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;

    private const string CallbackUrl = "https://www.mymerchant.co.nz/callback";

    private static readonly TimeZoneInfo NzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

    private static readonly Dictionary<string, string?> RequestHeaders = new Dictionary<string, string?>();

    private readonly EnduringConsentsApi _enduringConsentsApi;

    private readonly SingleConsentsApi _singleConsentsApi;

    private readonly PaymentsApi _paymentsApi;

    private readonly RefundsApi _instance;

    public RefundsApiTests(BlinkDebitFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _instance = fixture.RefundsApi;
        _enduringConsentsApi = fixture.EnduringConsentsApi;
        _singleConsentsApi = fixture.SingleConsentsApi;
        _paymentsApi = fixture.PaymentsApi;

        RequestHeaders[BlinkDebitConstant.REQUEST_ID.GetValue()] = Guid.NewGuid().ToString();
        RequestHeaders[BlinkDebitConstant.CORRELATION_ID.GetValue()] = Guid.NewGuid().ToString();
        RequestHeaders[BlinkDebitConstant.CUSTOMER_IP.GetValue()] = "192.168.0.1";
        RequestHeaders[BlinkDebitConstant.CUSTOMER_USER_AGENT.GetValue()] = "demo-api-client";
        RequestHeaders[BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = Guid.NewGuid().ToString();
    }

    public void Dispose()
    {
        RequestHeaders.Clear();
    }

    /// <summary>
    /// Test an instance of RefundsApi
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<RefundsApi>(_instance);
    }

    /// <summary>
    /// Verify that account number refund for single consent with decoupled flow is created and retrieved
    /// </summary>
    [Fact(DisplayName =
        "Verify that account number refund for single consent with decoupled flow is created and retrieved")]
    public async void CreateAccountNumberRefundForSingleConsentWithDecoupledFlow()
    {
        // create single consent with decoupled flow
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _singleConsentsApi.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);
        var consentId = createConsentResponse.ConsentId;
        Assert.True(string.IsNullOrEmpty(createConsentResponse.RedirectUri));

        var paymentId = Guid.Empty;
        RequestHeaders[BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = Guid.NewGuid().ToString();
        for (var i = 1; i <= 10; i++)
        {
            _testOutputHelper.WriteLine($"attempt: {i}");
            try
            {
                // create payment
                var paymentRequest = new PaymentRequest
                {
                    ConsentId = consentId
                };

                var paymentResponse = await _paymentsApi.CreatePaymentAsync(RequestHeaders, paymentRequest);

                Assert.NotNull(paymentResponse);
                paymentId = paymentResponse.PaymentId;

                break;
            }
            catch (Exception)
            {
                // sleep incrementally
                await Task.Delay(2000 * i);
            }
        }

        Assert.NotEqual(Guid.Empty, paymentId);

        // create account number refund
        var refundRequest = new AccountNumberRefundRequest(paymentId);

        var refundResponse = await _instance.CreateRefundAsync(RequestHeaders, refundRequest);

        Assert.NotNull(refundResponse);
        var refundId = refundResponse.RefundId;
        Assert.NotEqual(Guid.Empty, refundId);

        // retrieve account number refund
        var refund = await _instance.GetRefundAsync(refundId, RequestHeaders);

        Assert.Equal(Refund.StatusEnum.Completed, refund.Status);
        Assert.NotNull(refund.AccountNumber);
        Assert.StartsWith("99-", refund.AccountNumber);
        Assert.EndsWith("-00", refund.AccountNumber);
        Assert.NotEqual(DateTimeOffset.MinValue, refund.CreationTimestamp);
        Assert.NotEqual(DateTimeOffset.MinValue, refund.StatusUpdatedTimestamp);
        refundRequest = refund.Detail.GetAccountNumberRefundRequest();
        Assert.Equal(paymentId, refundRequest.PaymentId);
        Assert.Equal(RefundDetail.TypeEnum.AccountNumber, refundRequest.Type);
    }

    /// <summary>
    /// Verify that account number refund for enduring consent with decoupled flow is created and retrieved
    /// </summary>
    [Fact(DisplayName =
        "Verify that account number refund for enduring consent with decoupled flow is created and retrieved")]
    public async void CreateAccountNumberRefundForEnduringConsentWithDecoupledFlow()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var maximumAmount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, maximumAmount, hashedCustomerIdentifier);

        var createConsentResponse = await _enduringConsentsApi.CreateEnduringConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);
        var consentId = createConsentResponse.ConsentId;
        Assert.True(string.IsNullOrEmpty(createConsentResponse.RedirectUri));

        var paymentId = Guid.Empty;
        RequestHeaders[BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = Guid.NewGuid().ToString();
        for (var i = 1; i <= 10; i++)
        {
            _testOutputHelper.WriteLine($"attempt: {i}");
            try
            {
                var pcr = new Pcr("particulars", "code", "reference");
                var amount = new Amount("45.00", Amount.CurrencyEnum.NZD);
                var enduringPaymentRequest = new EnduringPaymentRequest(pcr, amount);
                var paymentRequest = new PaymentRequest(consentId, enduringPaymentRequest);

                var paymentResponse = await _paymentsApi.CreatePaymentAsync(RequestHeaders, paymentRequest);

                Assert.NotNull(paymentResponse);
                paymentId = paymentResponse.PaymentId;

                break;
            }
            catch (Exception)
            {
                // sleep incrementally
                await Task.Delay(2000 * i);
            }
        }

        Assert.NotEqual(Guid.Empty, paymentId);

        // create account number refund
        var refundRequest = new AccountNumberRefundRequest(paymentId);

        var refundResponse = await _instance.CreateRefundAsync(RequestHeaders, refundRequest);

        Assert.NotNull(refundResponse);
        var refundId = refundResponse.RefundId;
        Assert.NotEqual(Guid.Empty, refundId);

        // retrieve account number refund
        var refund = await _instance.GetRefundAsync(refundId, RequestHeaders);

        Assert.Equal(Refund.StatusEnum.Completed, refund.Status);
        Assert.NotNull(refund.AccountNumber);
        Assert.StartsWith("99-", refund.AccountNumber);
        Assert.EndsWith("-00", refund.AccountNumber);
        Assert.NotEqual(DateTimeOffset.MinValue, refund.CreationTimestamp);
        Assert.NotEqual(DateTimeOffset.MinValue, refund.StatusUpdatedTimestamp);
        refundRequest = refund.Detail.GetAccountNumberRefundRequest();
        Assert.Equal(paymentId, refundRequest.PaymentId);
        Assert.Equal(RefundDetail.TypeEnum.AccountNumber, refundRequest.Type);
    }

    /// <summary>
    /// Verify that full refund for single consent with decoupled flow is created and retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that full refund for single consent with decoupled flow is created and retrieved")]
    public async void CreateFullRefundForSingleConsentWithDecoupledFlow()
    {
        // create single consent with decoupled flow
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _singleConsentsApi.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);
        var consentId = createConsentResponse.ConsentId;
        Assert.True(string.IsNullOrEmpty(createConsentResponse.RedirectUri));

        var paymentId = Guid.Empty;
        RequestHeaders[BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = Guid.NewGuid().ToString();
        for (var i = 1; i <= 10; i++)
        {
            _testOutputHelper.WriteLine($"attempt: {i}");
            try
            {
                // create payment
                var paymentRequest = new PaymentRequest
                {
                    ConsentId = consentId
                };

                var paymentResponse = await _paymentsApi.CreatePaymentAsync(RequestHeaders, paymentRequest);

                Assert.NotNull(paymentResponse);
                paymentId = paymentResponse.PaymentId;

                break;
            }
            catch (Exception)
            {
                // sleep incrementally
                await Task.Delay(2000 * i);
            }
        }

        Assert.NotEqual(Guid.Empty, paymentId);

        // create full refund
        var refundRequest = new FullRefundRequest(paymentId, pcr, CallbackUrl);

        try
        {
            await _instance.CreateRefundAsync(RequestHeaders, refundRequest);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkNotImplementedException>(e);
            Assert.Equal("Full refund is not yet implemented", e.Message);
        }
    }

    /// <summary>
    /// Verify that partial refund for single consent with decoupled flow is created and retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that partial refund for single consent with decoupled flow is created and retrieved")]
    public async void CreatePartialRefundForSingleConsentWithDecoupledFlow()
    {
        // create single consent with decoupled flow
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _singleConsentsApi.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);
        var consentId = createConsentResponse.ConsentId;
        Assert.True(string.IsNullOrEmpty(createConsentResponse.RedirectUri));

        var paymentId = Guid.Empty;
        RequestHeaders[BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = Guid.NewGuid().ToString();
        for (var i = 1; i <= 10; i++)
        {
            _testOutputHelper.WriteLine($"attempt: {i}");
            try
            {
                // create payment
                var paymentRequest = new PaymentRequest
                {
                    ConsentId = consentId
                };

                var paymentResponse = await _paymentsApi.CreatePaymentAsync(RequestHeaders, paymentRequest);

                Assert.NotNull(paymentResponse);
                paymentId = paymentResponse.PaymentId;

                break;
            }
            catch (Exception)
            {
                // sleep incrementally
                await Task.Delay(2000 * i);
            }
        }

        Assert.NotEqual(Guid.Empty, paymentId);

        // create partial refund
        amount = new Amount("25.00", Amount.CurrencyEnum.NZD);
        var refundRequest = new PartialRefundRequest(paymentId, amount, pcr, CallbackUrl);

        try
        {
            await _instance.CreateRefundAsync(RequestHeaders, refundRequest);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkNotImplementedException>(e);
            Assert.Equal("Partial refund is not yet implemented", e.Message);
        }
    }
}