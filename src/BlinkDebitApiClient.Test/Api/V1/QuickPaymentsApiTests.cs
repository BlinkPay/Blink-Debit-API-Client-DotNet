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
using BlinkDebitApiClient.Model.V1;
using Xunit;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
///  Class for testing QuickPaymentsApi
/// </summary>
[Collection("Blink Debit Collection")]
public class QuickPaymentsApiTests : IDisposable
{
    private const string RedirectUri = "https://www.blinkpay.co.nz/sample-merchant-return-page";

    private const string CallbackUrl = "https://www.mymerchant.co.nz/callback";

    private readonly QuickPaymentsApi _instance;

    public QuickPaymentsApiTests(BlinkDebitFixture fixture)
    {
        _instance = fixture.QuickPaymentsApi;
    }

    public void Dispose()
    {
        // Cleanup when everything is done.
    }

    /// <summary>
    /// Test an instance of QuickPaymentsApi
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<QuickPaymentsApi>(_instance);
    }

    /// <summary>
    /// Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ")]
    public async void QuickPaymentWithRedirectFlowInPnz()
    {
        // create
        var redirectFlow = new RedirectFlow(RedirectUri, Bank.PNZ);
        var authFlowDetail = new AuthFlowDetail(redirectFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = await _instance.CreateQuickPaymentAsync(Guid.NewGuid(),
            Guid.NewGuid(), "192.168.0.1", Guid.NewGuid(), request);

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
        var quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        var consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.AwaitingAuthorisation, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetRedirectFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Redirect, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(RedirectUri, flow.RedirectUri);

        // revoke
        await _instance.RevokeQuickPaymentAsync(quickPaymentId);

        quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        flow = detail.Flow.Detail.GetRedirectFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Redirect, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(RedirectUri, flow.RedirectUri);
    }

    /// <summary>
    /// Verify that rejected quick payment with redirect flow is retrieved from PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that rejected quick payment with redirect flow is retrieved from PNZ")]
    public async void GetRejectedQuickPaymentWithRedirectFlowFromPnz()
    {
        var quickPayment = await _instance.GetQuickPaymentAsync(Guid.Parse("057a08f7-4ee1-499d-8726-e4fe802d64fc"));

        Assert.NotNull(quickPayment);
        var consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Rejected, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetRedirectFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Redirect, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(RedirectUri, flow.RedirectUri);
    }

    /// <summary>
    /// Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that quick payment with decoupled flow is created, retrieved and revoked in PNZ")]
    public async void QuickPaymentWithDecoupledFlowInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = await _instance.CreateQuickPaymentAsync(Guid.NewGuid(),
            Guid.NewGuid(), "192.168.0.1", Guid.NewGuid(), request);

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);

        // retrieve
        var quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        var consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.AwaitingAuthorisation, consent.Status);
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

        // revoke
        await _instance.RevokeQuickPaymentAsync(quickPaymentId);

        quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        detail = consent.Detail.GetSingleConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+6449144425", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ")]
    public async void QuickPaymentWithGatewayFlowAndRedirectFlowHintInPnz()
    {
        // create
        var redirectFlowHint = new RedirectFlowHint(Bank.PNZ);
        var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
        var gatewayFlow = new GatewayFlow(RedirectUri, flowHint);
        var authFlowDetail = new AuthFlowDetail(gatewayFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = await _instance.CreateQuickPaymentAsync(Guid.NewGuid(),
            Guid.NewGuid(), "192.168.0.1", Guid.NewGuid(), request);

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);
        Assert.NotEmpty(createQuickPaymentResponse.RedirectUri);
        Assert.EndsWith("/gateway/pay?id=" + quickPaymentId, createQuickPaymentResponse.RedirectUri);

        // retrieve
        var quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        var consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.GatewayAwaitingSubmission, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        pcr = detail.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = detail.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        gatewayFlow = detail.Flow.Detail.GetGatewayFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Gateway, gatewayFlow.Type);
        redirectFlowHint = gatewayFlow.FlowHint.GetRedirectFlowHint();
        Assert.Equal(Bank.PNZ, redirectFlowHint.Bank);
        Assert.Equal(RedirectUri, gatewayFlow.RedirectUri);

        // revoke
        await _instance.RevokeQuickPaymentAsync(quickPaymentId);

        quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        detail = consent.Detail.GetSingleConsentRequest();

        pcr = detail.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = detail.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        gatewayFlow = detail.Flow.Detail.GetGatewayFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Gateway, gatewayFlow.Type);
        redirectFlowHint = gatewayFlow.FlowHint.GetRedirectFlowHint();
        Assert.Equal(Bank.PNZ, redirectFlowHint.Bank);
        Assert.Equal(RedirectUri, gatewayFlow.RedirectUri);
    }

    /// <summary>
    /// Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that quick payment with redirect flow is created, retrieved and revoked in PNZ")]
    public async void QuickPaymentWithGatewayFlowAndDecoupledFlowHintInPnz()
    {
        // create
        var decoupledFlowHint = new DecoupledFlowHint(Bank.PNZ, IdentifierType.PhoneNumber, "+6449144425");
        var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
        var gatewayFlow = new GatewayFlow(RedirectUri, flowHint);
        var authFlowDetail = new AuthFlowDetail(gatewayFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        var createQuickPaymentResponse = await _instance.CreateQuickPaymentAsync(Guid.NewGuid(),
            Guid.NewGuid(), "192.168.0.1", Guid.NewGuid(), request);

        Assert.NotNull(createQuickPaymentResponse);

        var quickPaymentId = createQuickPaymentResponse.QuickPaymentId;
        Assert.NotEqual(Guid.Empty, quickPaymentId);
        Assert.NotEmpty(createQuickPaymentResponse.RedirectUri);
        Assert.EndsWith("/gateway/pay?id=" + quickPaymentId, createQuickPaymentResponse.RedirectUri);

        // retrieve
        var quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        var consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.GatewayAwaitingSubmission, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetSingleConsentRequest();

        pcr = detail.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = detail.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        gatewayFlow = detail.Flow.Detail.GetGatewayFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Gateway, gatewayFlow.Type);
        decoupledFlowHint = gatewayFlow.FlowHint.GetDecoupledFlowHint();
        Assert.Equal(Bank.PNZ, decoupledFlowHint.Bank);
        Assert.Equal(RedirectUri, gatewayFlow.RedirectUri);

        // revoke
        await _instance.RevokeQuickPaymentAsync(quickPaymentId);

        quickPayment = await _instance.GetQuickPaymentAsync(quickPaymentId);

        Assert.NotNull(quickPayment);
        consent = quickPayment.Consent;
        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        detail = consent.Detail.GetSingleConsentRequest();

        pcr = detail.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = detail.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        gatewayFlow = detail.Flow.Detail.GetGatewayFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Gateway, gatewayFlow.Type);
        decoupledFlowHint = gatewayFlow.FlowHint.GetDecoupledFlowHint();
        Assert.Equal(Bank.PNZ, decoupledFlowHint.Bank);
        Assert.Equal(RedirectUri, gatewayFlow.RedirectUri);
    }
}