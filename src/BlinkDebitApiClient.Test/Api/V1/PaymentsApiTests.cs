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
using System.Threading.Tasks;
using BlinkDebitApiClient.Api.V1;
using BlinkDebitApiClient.Model.V1;
using Xunit;
using Xunit.Abstractions;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
///  Class for testing PaymentsApi
/// </summary>
[Collection("Blink Debit Collection")]
public class PaymentsApiTests : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;

    private const string CallbackUrl = "https://www.mymerchant.co.nz/callback";

    private static readonly TimeZoneInfo NzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

    private readonly EnduringConsentsApi _enduringConsentsApi;

    private readonly SingleConsentsApi _singleConsentsApi;

    private readonly PaymentsApi _instance;

    public PaymentsApiTests(BlinkDebitFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _instance = fixture.PaymentsApi;
        _enduringConsentsApi = fixture.EnduringConsentsApi;
        _singleConsentsApi = fixture.SingleConsentsApi;
    }

    public void Dispose()
    {
        // Cleanup when everything is done.
    }

    /// <summary>
    /// Test an instance of PaymentsApi
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<PaymentsApi>(_instance);
    }

    /// <summary>
    /// Verify that payment for single consent with decoupled flow is created and retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that payment for single consent with decoupled flow is created and retrieved")]
    public async void CreatePaymentForSingleConsentWithDecoupledFlow()
    {
        // create single consent with decoupled flow
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _singleConsentsApi.CreateSingleConsentAsync(Guid.NewGuid(),
            Guid.NewGuid(), "192.168.0.1", Guid.NewGuid(), request);

        Assert.NotNull(createConsentResponse);
        var consentId = createConsentResponse.ConsentId;
        Assert.True(string.IsNullOrEmpty(createConsentResponse.RedirectUri));

        var paymentId = Guid.Empty;
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

                var paymentResponse = await _instance.CreatePaymentAsync(Guid.NewGuid(), Guid.NewGuid(),
                    Guid.NewGuid(), paymentRequest);

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

        // retrieve payment
        var payment = await _instance.GetPaymentAsync(paymentId);

        Assert.NotNull(payment);
        Assert.Equal(paymentId, payment.PaymentId);
        Assert.Equal(Payment.TypeEnum.Single, payment.Type);
        Assert.Equal(Payment.StatusEnum.AcceptedSettlementCompleted, payment.Status);
        Assert.Equal(Payment.AcceptedReasonEnum.SourceBankPaymentSent, payment.AcceptedReason);
        Assert.Empty(payment.Refunds);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.CreationTimestamp);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.StatusUpdatedTimestamp);
        var detail = payment.Detail;
        Assert.NotNull(detail);
        Assert.Equal(consentId, detail.ConsentId);
        Assert.Equal(Guid.Empty, detail.AccountReferenceId);
        Assert.Null(detail.EnduringPayment);
    }

    /// <summary>
    /// Verify that payment for enduring consent with decoupled flow is created and retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that payment for enduring consent with decoupled flow is created and retrieved")]
    public async void CreatePaymentForEnduringConsentWithDecoupledFlow()
    {
        // create enduring consent with decoupled flow
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var maximumAmount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, maximumAmount, hashedCustomerIdentifier);

        var createConsentResponse = await _enduringConsentsApi.CreateEnduringConsentAsync(Guid.NewGuid(),
            Guid.NewGuid(), "192.168.0.1", Guid.NewGuid(), request);


        Assert.NotNull(createConsentResponse);
        var consentId = createConsentResponse.ConsentId;
        Assert.True(string.IsNullOrEmpty(createConsentResponse.RedirectUri));

        var paymentId = Guid.Empty;
        for (var i = 1; i <= 10; i++)
        {
            _testOutputHelper.WriteLine($"attempt: {i}");
            try
            {
                // create payment
                var pcr = new Pcr("particulars", "code", "reference");
                var amount = new Amount("45.00", Amount.CurrencyEnum.NZD);
                var enduringPaymentRequest = new EnduringPaymentRequest(pcr, amount);
                var paymentRequest = new PaymentRequest(consentId, enduringPaymentRequest);

                var paymentResponse = await _instance.CreatePaymentAsync(Guid.NewGuid(), Guid.NewGuid(),
                    Guid.NewGuid(), paymentRequest);

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

        // retrieve payment
        var payment = await _instance.GetPaymentAsync(paymentId);

        Assert.NotNull(payment);
        Assert.Equal(paymentId, payment.PaymentId);
        Assert.Equal(Payment.TypeEnum.Enduring, payment.Type);
        Assert.Equal(Payment.StatusEnum.AcceptedSettlementCompleted, payment.Status);
        Assert.Equal(Payment.AcceptedReasonEnum.SourceBankPaymentSent, payment.AcceptedReason);
        Assert.Empty(payment.Refunds);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.CreationTimestamp);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.StatusUpdatedTimestamp);
        var detail = payment.Detail;
        Assert.NotNull(detail);
        Assert.Equal(consentId, detail.ConsentId);
        Assert.Equal(Guid.Empty, detail.AccountReferenceId);
        Assert.NotNull(detail.EnduringPayment);
    }
}