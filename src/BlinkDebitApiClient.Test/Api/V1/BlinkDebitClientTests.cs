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
using BlinkDebitApiClient.Api.V1;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Xunit;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
///  Class for testing BankMetadataApi
/// </summary>
[Collection("Blink Debit Collection")]
public class BlinkDebitClientTests : IDisposable
{
    private const string RedirectUri = "https://www.blinkpay.co.nz/sample-merchant-return-page";

    private const string CallbackUrl = "https://www.mymerchant.co.nz/callback";

    private static readonly TimeZoneInfo NzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

    private BlinkDebitClient _instance;

    public BlinkDebitClientTests(BlinkDebitFixture fixture)
    {
        _instance = fixture.BlinkDebitClient;
    }

    public void Dispose()
    {
        // Cleanup when everything is done.
    }

    /// <summary>
    /// Test an instance of BlinkDebitClient
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<BlinkDebitClient>(_instance);
    }

    /// <summary>
    /// Verify that bank metadata is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that bank metadata is retrieved")]
    public async void GetMeta()
    {
        var response = await _instance.GetMetaAsync(Guid.NewGuid(), Guid.NewGuid());
        Assert.IsType<List<BankMetadata>>(response);
        Assert.Equal(5, response.Count);
    }

    /// <summary>
    /// Verify that timed out single consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out single consent is handled")]
    public void AwaitTimedOutSingleConsentThenThrowRuntimeException()
    {
        // create
        var redirectFlow = new RedirectFlow(RedirectUri, Bank.PNZ);
        var authFlowDetail = new AuthFlowDetail(redirectFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);
        Assert.NotEmpty(createConsentResponse.RedirectUri);
        Assert.StartsWith("https://api-nomatls.apicentre.middleware.co.nz/middleware-nz-sandbox/v2.0/oauth/authorize",
            createConsentResponse.RedirectUri);
        Assert.Contains("scope=openid%20payments&response_type=code%20id_token", createConsentResponse.RedirectUri);
        Assert.Contains("&request=", createConsentResponse.RedirectUri);
        Assert.Contains("&state=", createConsentResponse.RedirectUri);
        Assert.Contains("&nonce=", createConsentResponse.RedirectUri);
        Assert.Contains("&redirect_uri=", createConsentResponse.RedirectUri);
        Assert.Contains("&client_id=", createConsentResponse.RedirectUri);

        // retrieve
        try
        {
            _instance.AwaitAuthorisedSingleConsent(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent single consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent single consent is handled")]
    public void AwaitNonExistentSingleConsentThenThrowRuntimeException()
    {
        var consentId = Guid.NewGuid();

        try
        {
            _instance.AwaitAuthorisedSingleConsent(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent with ID [" + consentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that single consent with decoupled flow is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that single consent with decoupled flow is retrieved")]
    public void AwaitAuthorisedSingleConsent()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = _instance.AwaitAuthorisedSingleConsent(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that timed out single consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out single consent is handled")]
    public void AwaitTimedOutSingleConsentThenThrowConsentTimeoutException()
    {
        // create
        var redirectFlow = new RedirectFlow(RedirectUri, Bank.PNZ);
        var authFlowDetail = new AuthFlowDetail(redirectFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);
        Assert.NotEmpty(createConsentResponse.RedirectUri);
        Assert.StartsWith("https://api-nomatls.apicentre.middleware.co.nz/middleware-nz-sandbox/v2.0/oauth/authorize",
            createConsentResponse.RedirectUri);
        Assert.Contains("scope=openid%20payments&response_type=code%20id_token", createConsentResponse.RedirectUri);
        Assert.Contains("&request=", createConsentResponse.RedirectUri);
        Assert.Contains("&state=", createConsentResponse.RedirectUri);
        Assert.Contains("&nonce=", createConsentResponse.RedirectUri);
        Assert.Contains("&redirect_uri=", createConsentResponse.RedirectUri);
        Assert.Contains("&client_id=", createConsentResponse.RedirectUri);

        // retrieve
        try
        {
            _instance.AwaitAuthorisedSingleConsentOrThrowException(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent single consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent single consent is handled")]
    public void AwaitNonExistentSingleConsentThenThrowResourceNotFoundException()
    {
        var consentId = Guid.NewGuid();

        try
        {
            _instance.AwaitAuthorisedSingleConsentOrThrowException(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent with ID [" + consentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that single consent with decoupled flow is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that single consent with decoupled flow is retrieved")]
    public void AwaitAuthorisedSingleConsentOrThrowException()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = _instance.AwaitAuthorisedSingleConsentOrThrowException(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that timed out enduring consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out enduring consent is handled")]
    public void AwaitTimedOutEnduringConsentThenThrowRuntimeException()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var amount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, amount, ConsentDetail.TypeEnum.Enduring);

        var createConsentResponse = _instance.CreateEnduringConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        try
        {
            _instance.AwaitAuthorisedEnduringConsent(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent enduring consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent enduring consent is handled")]
    public void AwaitNonExistentEnduringConsentThenThrowRuntimeException()
    {
        var consentId = Guid.NewGuid();

        try
        {
            _instance.AwaitAuthorisedEnduringConsent(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent with ID [" + consentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that enduring consent with decoupled flow is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that enduring consent with decoupled flow is retrieved")]
    public void AwaitAuthorisedEnduringConsent()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var amount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, amount, ConsentDetail.TypeEnum.Enduring);

        var createConsentResponse = _instance.CreateEnduringConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = _instance.AwaitAuthorisedEnduringConsent(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
        Assert.Equal(Period.Fortnightly, detail.Period);
        Assert.NotNull(detail.MaximumAmountPeriod);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPeriod.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPeriod.Total);
    }

    /// <summary>
    /// Verify that timed out enduring consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out enduring consent is handled")]
    public void AwaitTimedOutEnduringConsentThenThrowConsentTimeoutException()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var amount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, amount, ConsentDetail.TypeEnum.Enduring);

        var createConsentResponse = _instance.CreateEnduringConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        try
        {
            _instance.AwaitAuthorisedEnduringConsentOrThrowException(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent enduring consent is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent enduring consent is handled")]
    public void AwaitNonExistentEnduringConsentThenThrowResourceNotFoundException()
    {
        var consentId = Guid.NewGuid();

        try
        {
            _instance.AwaitAuthorisedEnduringConsentOrThrowException(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent with ID [" + consentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that enduring consent with decoupled flow is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that enduring consent with decoupled flow is retrieved")]
    public void AwaitAuthorisedEnduringConsentOrThrowException()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var amount = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, amount, ConsentDetail.TypeEnum.Enduring);

        var createConsentResponse = _instance.CreateEnduringConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = _instance.AwaitAuthorisedEnduringConsentOrThrowException(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
        Assert.Equal(Period.Fortnightly, detail.Period);
        Assert.NotNull(detail.MaximumAmountPeriod);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPeriod.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPeriod.Total);
    }

    /// <summary>
    /// Verify that timed out quick payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out quick payment is handled")]
    public void AwaitTimedOutQuickPaymentThenThrowRuntimeException()
    {
        // create
        var redirectFlow = new RedirectFlow(RedirectUri, Bank.PNZ);
        var authFlowDetail = new AuthFlowDetail(redirectFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = _instance.CreateQuickPayment(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);
        Assert.NotEmpty(createQuickPaymentResponse.RedirectUri);
        Assert.StartsWith("https://api-nomatls.apicentre.middleware.co.nz/middleware-nz-sandbox/v2.0/oauth/authorize",
            createQuickPaymentResponse.RedirectUri);
        Assert.Contains("scope=openid%20payments&response_type=code%20id_token",
            createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&request=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&state=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&nonce=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&redirect_uri=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&client_id=", createQuickPaymentResponse.RedirectUri);

        // retrieve
        try
        {
            _instance.AwaitSuccessfulQuickPayment(quickPaymentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent quick payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent quick payment is handled")]
    public void AwaitNonExistentQuickPaymentThenThrowRuntimeException()
    {
        var consentId = Guid.NewGuid();

        try
        {
            _instance.AwaitSuccessfulQuickPayment(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent with ID [" + consentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that quick payment with decoupled flow is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that quick payment with decoupled flow is retrieved", Skip = "Fails in GitHub")]
    public void AwaitSuccessfulQuickPayment()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = _instance.CreateQuickPayment(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);

        // retrieve
        var quickPaymentResponse = _instance.AwaitSuccessfulQuickPayment(quickPaymentId, 300);

        Assert.NotNull(quickPaymentResponse);
        Assert.Contains(quickPaymentResponse.Consent.Status,
            new[] { Consent.StatusEnum.Consumed, Consent.StatusEnum.Authorised });
        var payments = quickPaymentResponse.Consent.Payments;
        if (payments.Count > 0)
        {
            var payment = payments[0];
            Assert.NotNull(payment);
            Assert.NotEqual(Guid.Empty, payment.PaymentId);
            Assert.Equal(Payment.TypeEnum.Single, payment.Type);
            Assert.Equal(Payment.StatusEnum.AcceptedSettlementCompleted, payment.Status);
            Assert.Equal(Payment.AcceptedReasonEnum.SourceBankPaymentSent, payment.AcceptedReason);
            Assert.Empty(payment.Refunds);
            Assert.NotEqual(DateTimeOffset.MinValue, payment.CreationTimestamp);
            Assert.NotEqual(DateTimeOffset.MinValue, payment.StatusUpdatedTimestamp);
            var paymentDetail = payment.Detail;
            Assert.NotNull(paymentDetail);
            Assert.Equal(quickPaymentId, paymentDetail.ConsentId);
            Assert.Equal(Guid.Empty, paymentDetail.AccountReferenceId);
        }

        Assert.NotNull(quickPaymentResponse.Consent.Detail);
        Assert.IsType<ConsentDetail>(quickPaymentResponse.Consent.Detail);
        var detail = quickPaymentResponse.Consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, quickPaymentResponse.Consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that timed out quick payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out quick payment is handled")]
    public void AwaitTimedOutQuickPaymentThenThrowConsentTimeoutException()
    {
        // create
        var redirectFlow = new RedirectFlow(RedirectUri, Bank.PNZ);
        var authFlowDetail = new AuthFlowDetail(redirectFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = _instance.CreateQuickPayment(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);
        Assert.NotEmpty(createQuickPaymentResponse.RedirectUri);
        Assert.StartsWith("https://api-nomatls.apicentre.middleware.co.nz/middleware-nz-sandbox/v2.0/oauth/authorize",
            createQuickPaymentResponse.RedirectUri);
        Assert.Contains("scope=openid%20payments&response_type=code%20id_token",
            createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&request=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&state=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&nonce=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&redirect_uri=", createQuickPaymentResponse.RedirectUri);
        Assert.Contains("&client_id=", createQuickPaymentResponse.RedirectUri);

        // retrieve
        try
        {
            _instance.AwaitSuccessfulQuickPaymentOrThrowException(quickPaymentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent quick payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent quick payment is handled")]
    public void AwaitNonExistentQuickPaymentThenThrowResourceNotFoundException()
    {
        var consentId = Guid.NewGuid();

        try
        {
            _instance.AwaitSuccessfulQuickPaymentOrThrowException(consentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Consent with ID [" + consentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that quick payment with decoupled flow is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that quick payment with decoupled flow is retrieved")]
    public void AwaitSuccessfulQuickPaymentOrThrowException()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = _instance.CreateQuickPayment(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);

        // retrieve
        var quickPaymentResponse = _instance.AwaitSuccessfulQuickPaymentOrThrowException(quickPaymentId, 300);

        Assert.NotNull(quickPaymentResponse);
        Assert.Contains(quickPaymentResponse.Consent.Status,
            new[] { Consent.StatusEnum.Consumed, Consent.StatusEnum.Authorised });
        // Assert.Equal(Consent.StatusEnum.Consumed, quickPaymentResponse.Consent.Status);
        var payments = quickPaymentResponse.Consent.Payments;
        if (payments.Count > 0)
        {
            var payment = payments[0];
            Assert.NotNull(payment);
            Assert.NotEqual(Guid.Empty, payment.PaymentId);
            Assert.Equal(Payment.TypeEnum.Single, payment.Type);
            Assert.Equal(Payment.StatusEnum.AcceptedSettlementCompleted, payment.Status);
            Assert.Equal(Payment.AcceptedReasonEnum.SourceBankPaymentSent, payment.AcceptedReason);
            Assert.Empty(payment.Refunds);
            Assert.NotEqual(DateTimeOffset.MinValue, payment.CreationTimestamp);
            Assert.NotEqual(DateTimeOffset.MinValue, payment.StatusUpdatedTimestamp);
            var paymentDetail = payment.Detail;
            Assert.NotNull(paymentDetail);
            Assert.Equal(quickPaymentId, paymentDetail.ConsentId);
            Assert.Equal(Guid.Empty, paymentDetail.AccountReferenceId);
        }

        Assert.NotNull(quickPaymentResponse.Consent.Detail);
        Assert.IsType<ConsentDetail>(quickPaymentResponse.Consent.Detail);
        var detail = quickPaymentResponse.Consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, quickPaymentResponse.Consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that timed out payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out payment is handled")]
    public void AwaitTimedOutPaymentThenThrowRuntimeException()
    {
        // create consent
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve consent
        var consent = _instance.AwaitAuthorisedSingleConsent(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);

        // create payment
        var paymentRequest = new PaymentRequest
        {
            ConsentId = consentId
        };

        var paymentResponse = _instance.CreatePayment(paymentRequest, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        Assert.NotNull(paymentResponse);
        var paymentId = paymentResponse.PaymentId;

        // retrieve payment
        try
        {
            _instance.AwaitSuccessfulPayment(paymentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Payment timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent payment is handled")]
    public void AwaitNonExistentPaymentThenThrowRuntimeException()
    {
        var paymentId = Guid.NewGuid();

        try
        {
            _instance.AwaitSuccessfulPayment(paymentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Payment with ID [" + paymentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that payment is created and retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that payment is created and retrieved")]
    public void AwaitSuccessfulPayment()
    {
        // create consent
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve consent
        var consent = _instance.AwaitAuthorisedSingleConsent(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);

        // create payment
        var paymentRequest = new PaymentRequest
        {
            ConsentId = consentId
        };

        var paymentResponse = _instance.CreatePayment(paymentRequest, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        Assert.NotNull(paymentResponse);
        var paymentId = paymentResponse.PaymentId;

        // retrieve payment
        var payment = _instance.AwaitSuccessfulPayment(paymentId, 10);

        Assert.NotNull(payment);
        Assert.Equal(paymentId, payment.PaymentId);
        Assert.Equal(Payment.TypeEnum.Single, payment.Type);
        Assert.Equal(Payment.StatusEnum.AcceptedSettlementCompleted, payment.Status);
        Assert.Equal(Payment.AcceptedReasonEnum.SourceBankPaymentSent, payment.AcceptedReason);
        Assert.Empty(payment.Refunds);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.CreationTimestamp);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.StatusUpdatedTimestamp);
        var paymentDetail = payment.Detail;
        Assert.NotNull(paymentDetail);
        Assert.Equal(consentId, paymentDetail.ConsentId);
        Assert.Equal(Guid.Empty, paymentDetail.AccountReferenceId);
        Assert.Null(paymentDetail.EnduringPayment);
    }

    /// <summary>
    /// Verify that timed out payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that timed out payment is handled")]
    public void AwaitTimedOutPaymentThenThrowPaymentTimeoutException()
    {
        // create consent
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve consent
        var consent = _instance.AwaitAuthorisedSingleConsent(consentId, 300);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);

        // create payment
        var paymentRequest = new PaymentRequest
        {
            ConsentId = consentId
        };

        var paymentResponse = _instance.CreatePayment(paymentRequest, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        Assert.NotNull(paymentResponse);
        var paymentId = paymentResponse.PaymentId;

        // retrieve payment
        try
        {
            _instance.AwaitSuccessfulPayment(paymentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Payment timed out", e.Message);
        }
    }

    /// <summary>
    /// Verify that non-existent payment is handled
    /// </summary>
    [Fact(DisplayName = "Verify that non-existent payment is handled")]
    public void AwaitNonExistentPaymentThenThrowResourceNotFoundException()
    {
        var paymentId = Guid.NewGuid();

        try
        {
            _instance.AwaitSuccessfulPaymentOrThrowException(paymentId, 5);
        }
        catch (Exception e)
        {
            Assert.IsType<BlinkServiceException>(e);
            Assert.Equal("Payment with ID [" + paymentId + "] does not exist", e.Message);
        }
    }

    /// <summary>
    /// Verify that payment is created and retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that payment is created and retrieved")]
    public void AwaitAuthorisedPaymentOrThrowException()
    {
        // create consent
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        var createConsentResponse = _instance.CreateSingleConsent(request, Guid.NewGuid(), Guid.NewGuid(),
            "192.168.0.1", Guid.NewGuid());

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve consent
        var consent = _instance.AwaitAuthorisedSingleConsent(consentId, 30);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Authorised, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);

        // create payment
        var paymentRequest = new PaymentRequest
        {
            ConsentId = consentId
        };

        var paymentResponse = _instance.CreatePayment(paymentRequest, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        Assert.NotNull(paymentResponse);
        var paymentId = paymentResponse.PaymentId;

        // retrieve payment
        var payment = _instance.AwaitSuccessfulPaymentOrThrowException(paymentId, 10);

        Assert.NotNull(payment);
        Assert.Equal(paymentId, payment.PaymentId);
        Assert.Equal(Payment.TypeEnum.Single, payment.Type);
        Assert.Equal(Payment.StatusEnum.AcceptedSettlementCompleted, payment.Status);
        Assert.Equal(Payment.AcceptedReasonEnum.SourceBankPaymentSent, payment.AcceptedReason);
        Assert.Empty(payment.Refunds);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.CreationTimestamp);
        Assert.NotEqual(DateTimeOffset.MinValue, payment.StatusUpdatedTimestamp);
        var paymentDetail = payment.Detail;
        Assert.NotNull(paymentDetail);
        Assert.Equal(consentId, paymentDetail.ConsentId);
        Assert.Equal(Guid.Empty, paymentDetail.AccountReferenceId);
        Assert.Null(paymentDetail.EnduringPayment);
    }
}