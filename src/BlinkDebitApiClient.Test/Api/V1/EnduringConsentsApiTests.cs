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
///  Class for testing EnduringConsentsApi
/// </summary>
[Collection("Blink Debit Collection")]
public class EnduringConsentsApiTests : IDisposable
{
    private const string RedirectUri = "https://www.blinkpay.co.nz/sample-merchant-return-page";

    private const string CallbackUrl = "https://www.mymerchant.co.nz/callback";

    private static readonly TimeZoneInfo NzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

    private static readonly Dictionary<string, string?> RequestHeaders = new Dictionary<string, string?>();

    private readonly EnduringConsentsApi _instance;

    public EnduringConsentsApiTests(BlinkDebitFixture fixture)
    {
        _instance = fixture.EnduringConsentsApi;

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
    /// Test an instance of EnduringConsentsApi
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<EnduringConsentsApi>(_instance);
    }

    /// <summary>
    /// Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ")]
    public async Task EnduringConsentWithRedirectFlowInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var maximumAmountPeriod = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var maximumAmountPayment = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, maximumAmountPeriod, maximumAmountPayment, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateEnduringConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Contains(consent.Status, new[] { Consent.StatusEnum.AwaitingAuthorisation, Consent.StatusEnum.Authorised });
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
        Assert.Equal("+64-259531933", flow.IdentifierValue);
        Assert.Equal(Period.Fortnightly, detail.Period);
        Assert.NotNull(detail.MaximumAmountPeriod);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPeriod.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPeriod.Total);
        Assert.NotNull(detail.MaximumAmountPayment);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPayment.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPayment.Total);

        // revoke
        await _instance.RevokeEnduringConsentAsync(consentId);

        consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        detail = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+64-259531933", flow.IdentifierValue);
        Assert.Equal(Period.Fortnightly, detail.Period);
        Assert.NotNull(detail.MaximumAmountPeriod);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPeriod.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPeriod.Total);
        Assert.NotNull(detail.MaximumAmountPayment);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPayment.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPayment.Total);
    }

    /// <summary>
    /// Verify that rejected enduring consent with redirect flow is retrieved from PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that rejected enduring consent with redirect flow is retrieved from PNZ",
        Skip = "Disabled temporarily until a new rejected consent ID is supplied")]
    public async Task GetRejectedEnduringConsentWithRedirectFlowFromPnz()
    {
        var consent = await _instance.GetEnduringConsentAsync(Guid.Parse("f0b5fc9e-afa2-441f-a9e7-e2131952b835"));

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Rejected, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        var detail = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        var flow = detail.Flow.Detail.GetRedirectFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Redirect, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(RedirectUri, flow.RedirectUri);
    }

    /// <summary>
    /// Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that enduring consent with decoupled flow is created, retrieved and revoked in PNZ")]
    public async Task EnduringConsentWithDecoupledFlowInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933",
            CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var maximumAmountPeriod = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var maximumAmountPayment = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, maximumAmountPeriod, maximumAmountPayment, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateEnduringConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.AwaitingAuthorisation, consent.Status);
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
        Assert.Equal("+64-259531933", flow.IdentifierValue);
        Assert.Equal(Period.Fortnightly, detail.Period);
        Assert.NotNull(detail.MaximumAmountPeriod);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPeriod.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPeriod.Total);
        Assert.NotNull(detail.MaximumAmountPayment);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPayment.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPayment.Total);

        // revoke
        await _instance.RevokeEnduringConsentAsync(consentId);

        consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        detail = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);
        Assert.NotNull(detail.Flow);
        Assert.NotNull(detail.Flow.Detail);
        Assert.IsType<AuthFlowDetail>(detail.Flow.Detail);
        flow = detail.Flow.Detail.GetDecoupledFlow();

        Assert.Equal(AuthFlowDetail.TypeEnum.Decoupled, flow.Type);
        Assert.Equal(Bank.PNZ, flow.Bank);
        Assert.Equal(CallbackUrl, flow.CallbackUrl);
        Assert.Equal(IdentifierType.PhoneNumber, flow.IdentifierType);
        Assert.Equal("+64-259531933", flow.IdentifierValue);
        Assert.Equal(Period.Fortnightly, detail.Period);
        Assert.NotNull(detail.MaximumAmountPeriod);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPeriod.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPeriod.Total);
        Assert.NotNull(detail.MaximumAmountPayment);
        Assert.Equal(Amount.CurrencyEnum.NZD, detail.MaximumAmountPayment.Currency);
        Assert.Equal("50.00", detail.MaximumAmountPayment.Total);
    }

    /// <summary>
    /// Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ")]
    public async Task EnduringConsentWithGatewayFlowAndRedirectFlowHintInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var maximumAmountPeriod = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var maximumAmountPayment = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, maximumAmountPeriod, maximumAmountPayment, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateEnduringConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Contains(consent.Status, new[] { Consent.StatusEnum.AwaitingAuthorisation, Consent.StatusEnum.Authorised });
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(Period.Fortnightly, request.Period);

        maximumAmountPeriod = request.MaximumAmountPeriod;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPeriod.Currency);
        Assert.Equal("50.00", maximumAmountPeriod.Total);

        maximumAmountPayment = request.MaximumAmountPayment;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPayment.Currency);
        Assert.Equal("50.00", maximumAmountPayment.Total);

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
        await _instance.RevokeEnduringConsentAsync(consentId);

        consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(Period.Fortnightly, request.Period);

        maximumAmountPeriod = request.MaximumAmountPeriod;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPeriod.Currency);
        Assert.Equal("50.00", maximumAmountPeriod.Total);

        maximumAmountPayment = request.MaximumAmountPayment;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPayment.Currency);
        Assert.Equal("50.00", maximumAmountPayment.Total);

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
    /// Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ
    /// </summary>
    [Fact(DisplayName = "Verify that enduring consent with redirect flow is created, retrieved and revoked in PNZ")]
    public async Task EnduringConsentWithGatewayFlowAndDecoupledFlowHintInPnz()
    {
        // create
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", CallbackUrl);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var maximumAmountPeriod = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var maximumAmountPayment = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var fromTimestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
        var hashedCustomerIdentifier = "88df3798e32512ac340164f7ed133343d6dcb4888e4a91b03512dedd9800d12e";
        var request = new EnduringConsentRequest(authFlow, fromTimestamp, default,
            Period.Fortnightly, maximumAmountPeriod, maximumAmountPayment, hashedCustomerIdentifier);

        var createConsentResponse = await _instance.CreateEnduringConsentAsync(RequestHeaders, request);

        Assert.NotNull(createConsentResponse);

        var consentId = createConsentResponse.ConsentId;
        Assert.NotEqual(Guid.Empty, consentId);

        // retrieve
        var consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Contains(consent.Status, new[] { Consent.StatusEnum.AwaitingAuthorisation, Consent.StatusEnum.Authorised });
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(Period.Fortnightly, request.Period);

        maximumAmountPeriod = request.MaximumAmountPeriod;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPeriod.Currency);
        Assert.Equal("50.00", maximumAmountPeriod.Total);

        maximumAmountPayment = request.MaximumAmountPayment;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPayment.Currency);
        Assert.Equal("50.00", maximumAmountPayment.Total);

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
        await _instance.RevokeEnduringConsentAsync(consentId);

        consent = await _instance.GetEnduringConsentAsync(consentId);

        Assert.NotNull(consent);
        Assert.Equal(Consent.StatusEnum.Revoked, consent.Status);
        Assert.Empty(consent.Payments);
        Assert.Equal(ConsentDetail.TypeEnum.Enduring, consent.Detail.Type);

        Assert.NotNull(consent.Detail);
        Assert.IsType<ConsentDetail>(consent.Detail);
        request = consent.Detail.GetEnduringConsentRequest();

        Assert.Equal(Period.Fortnightly, request.Period);

        maximumAmountPeriod = request.MaximumAmountPeriod;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPeriod.Currency);
        Assert.Equal("50.00", maximumAmountPeriod.Total);

        maximumAmountPayment = request.MaximumAmountPayment;
        Assert.Equal(Amount.CurrencyEnum.NZD, maximumAmountPayment.Currency);
        Assert.Equal("50.00", maximumAmountPayment.Total);

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
}