/*
 * Copyright (c) 2025 BlinkPay
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
using BlinkDebitApiClient.Model.V1;
using Xunit;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
///  Class for testing SingleConsentsApi
/// </summary>
[Collection("Blink Debit Collection")]
public class SingleConsentsApiTests : IDisposable
{
    private const string RedirectUri = "https://www.blinkpay.co.nz/sample-merchant-return-page";

    private const string CallbackUrl = "https://www.mymerchant.co.nz/callback";

    private static readonly Dictionary<string, string?> RequestHeaders = new Dictionary<string, string?>();

    private readonly SingleConsentsApi _instance;

    public SingleConsentsApiTests(BlinkDebitFixture fixture)
    {
        _instance = fixture.SingleConsentsApi;


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
    /// Test an instance of SingleConsentsApi
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<SingleConsentsApi>(_instance);
    }

    /// <summary>
    /// Verify that single consent with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that single consent with redirect flow is created, retrieved and revoked in PNZ")]
    public async Task SingleConsentWithRedirectFlowInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetSingleConsentAsync(consentId);

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
        Assert.Equal("+64-259531933", flow.IdentifierValue);

        // revoke
        await _instance.RevokeSingleConsentAsync(consentId);

        consent = await _instance.GetSingleConsentAsync(consentId);

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
        Assert.Equal("+64-259531933", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that rejected single consent with redirect flow is retrieved from PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that rejected single consent with redirect flow is retrieved from PNZ")]
    public async Task GetRejectedSingleConsentWithRedirectFlowFromPnz()
    {
        var consent = await _instance.GetSingleConsentAsync(Guid.Parse("0a52bdff-4d63-4c21-ae4f-8ef438d74532"));

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
    /// Verify that single consent with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that single consent with decoupled flow is created, retrieved and revoked in PNZ")]
    public async Task SingleConsentWithDecoupledFlowInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetSingleConsentAsync(consentId);

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
        Assert.Equal("+64-259531933", flow.IdentifierValue);

        // revoke
        await _instance.RevokeSingleConsentAsync(consentId);

        consent = await _instance.GetSingleConsentAsync(consentId);

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
        Assert.Equal("+64-259531933", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that single consent with gateway flow and redirect flow hint is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName =
        "Verify that single consent with gateway flow and redirect flow hint is created, retrieved and revoked in PNZ")]
    public async Task SingleConsentWithGatewayFlowAndRedirectFlowHintInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetSingleConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Contains(consent.Status, new[] { Consent.StatusEnum.AwaitingAuthorisation, Consent.StatusEnum.Authorised });
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetSingleConsentRequest();

        pcr = request.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = request.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(request.Flow);
        Assert.NotNull(request.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(request.Flow.Detail);
        var flow = request.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+64-259531933", flow.IdentifierValue);

        // revoke
        await _instance.RevokeSingleConsentAsync(consentId);

        consent = await _instance.GetSingleConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetSingleConsentRequest();

        pcr = request.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = request.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(request.Flow);
        Assert.NotNull(request.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(request.Flow.Detail);
        flow = request.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+64-259531933", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that single consent with gateway flow and decoupled flow hint is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName =
        "Verify that single consent with gateway flow and decoupled flow hint is created, retrieved and revoked in PNZ")]
    public async Task SingleConsentWithGatewayFlowAndDecoupledFlowHintInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetSingleConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Contains(consent.Status, new[] { Consent.StatusEnum.AwaitingAuthorisation, Consent.StatusEnum.Authorised });
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetSingleConsentRequest();

        pcr = request.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = request.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(request.Flow);
        Assert.NotNull(request.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(request.Flow.Detail);
        var flow = request.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+64-259531933", flow.IdentifierValue);

        // revoke
        await _instance.RevokeSingleConsentAsync(consentId);

        consent = await _instance.GetSingleConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Single, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetSingleConsentRequest();

        pcr = request.Pcr;
        Assert.Equal("particulars", pcr.Particulars);
        Assert.Equal("code", pcr.Code);
        Assert.Equal("reference", pcr.Reference);

        amount = request.Amount;
        Assert.Equal(Amount.CurrencyEnum.NZD, amount.Currency);
        Assert.Equal("1.25", amount.Total);

        Assert.NotNull(request.Flow);
        Assert.NotNull(request.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(request.Flow.Detail);
        flow = request.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+64-259531933", flow.IdentifierValue);
    }

    /// <summary>
    /// Verify that single consent with redirect to app is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that single consent with redirect to app is created, retrieved and revoked in PNZ")]
    public async Task SingleConsentWithRedirectToAppInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("1.25", Amount.CurrencyEnum.NZD);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new SingleConsentRequest(authFlow, pcr, amount, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateSingleConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetSingleConsentAsync(consentId);

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
        Assert.Equal("+64-259531933", flow.IdentifierValue);

        // revoke
        await _instance.RevokeSingleConsentAsync(consentId);

        consent = await _instance.GetSingleConsentAsync(consentId);

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
        Assert.Equal("+64-259531933", flow.IdentifierValue);
    }
}