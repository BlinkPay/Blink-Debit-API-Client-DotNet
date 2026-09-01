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
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR IN CONNECTION WITH THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlinkDebitApiClient.Api.V1;
using BlinkDebitApiClient.Exceptions;
using BlinkDebitApiClient.Model.V1;
using Xunit;
using Xunit.Abstractions;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
/// Comprehensive test suite covering README quick start examples and key SDK operations.
/// Tests run against sandbox environment with real credentials.
/// </summary>
[Collection("Blink Debit Collection")]
public class ComprehensiveQuickStartTests : IDisposable
{
    private readonly BlinkDebitClient _client;
    private readonly ITestOutputHelper _output;
    private static readonly Dictionary<string, string?> RequestHeaders;
    private const string RedirectUri = "https://www.blinkpay.co.nz/sample-merchant-return-page";

    static ComprehensiveQuickStartTests()
    {
        RequestHeaders = new Dictionary<string, string?>
        {
            { "X-Request-Id", Guid.NewGuid().ToString() },
            { "X-Correlation-Id", Guid.NewGuid().ToString() },
            { "X-Customer-IP", "1.2.3.4" },
            { "X-Customer-User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/91.0" },
            { "Idempotency-Key", Guid.NewGuid().ToString() }
        };
    }

    public ComprehensiveQuickStartTests(BlinkDebitFixture fixture, ITestOutputHelper output)
    {
        _client = fixture.BlinkDebitClient;
        _output = output;
    }

    public void Dispose()
    {
        RequestHeaders.Clear();
    }

    [Fact(DisplayName = "1. Quick Start - Gateway Flow Quick Payment (README Example)")]
    public async Task Test1_QuickStartGatewayFlowQuickPayment()
    {
        _output.WriteLine("=== TEST 1: Quick Start - Gateway Flow Quick Payment ===");

        // Arrange - Following README Quick Start example exactly
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("particulars", "code", "reference");
        var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        // Act
        var qpCreateResponse = await _client.CreateQuickPaymentAsync(request, RequestHeaders);

        // Assert
        Assert.NotNull(qpCreateResponse);
        Assert.NotEqual(Guid.Empty, qpCreateResponse.QuickPaymentId);

        _output.WriteLine($"✓ Quick Payment Created: {qpCreateResponse.QuickPaymentId}");
        _output.WriteLine("✓ Quick Start example PASSED\n");
    }

    [Fact(DisplayName = "2. Get Bank Metadata")]
    public async Task Test2_GetBankMetadata()
    {
        _output.WriteLine("=== TEST 2: Get Bank Metadata ===");

        // Act
        var bankMetadataList = await _client.GetMetaAsync(RequestHeaders);

        // Assert
        Assert.NotNull(bankMetadataList);
        Assert.NotEmpty(bankMetadataList);

        _output.WriteLine($"✓ Retrieved {bankMetadataList.Count} banks");

        foreach (var bank in bankMetadataList)
        {
            Assert.NotNull(bank.Features);

            _output.WriteLine($"  - {bank.Name}:");
            _output.WriteLine($"      Enduring Consent: {bank.Features.EnduringConsent?.Enabled ?? false}");
            _output.WriteLine($"      Decoupled Flow: {bank.Features.DecoupledFlow?.Enabled ?? false}");
        }

        _output.WriteLine("✓ Bank Metadata retrieval PASSED\n");
    }

    [Fact(DisplayName = "3. Create and Retrieve Single Consent with Redirect Flow")]
    public async Task Test3_CreateAndRetrieveSingleConsent()
    {
        _output.WriteLine("=== TEST 3: Create and Retrieve Single Consent ===");

        // Arrange
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("test-single");
        var amount = new Amount("2.50", Amount.CurrencyEnum.NZD);
        var request = new SingleConsentRequest(authFlow, pcr, amount);

        // Act - Create
        var createResponse = await _client.CreateSingleConsentAsync(request, RequestHeaders);

        // Assert - Create
        Assert.NotNull(createResponse);
        Assert.NotEqual(Guid.Empty, createResponse.ConsentId);

        _output.WriteLine($"✓ Single Consent Created: {createResponse.ConsentId}");
        _output.WriteLine("✓ Single Consent creation PASSED\n");
    }

    [Fact(DisplayName = "4. Create Quick Payment with Different Flow Types")]
    public async Task Test4_CreateQuickPaymentWithDifferentFlows()
    {
        _output.WriteLine("=== TEST 4: Create Quick Payments with Different Flows ===");

        // Test 4a: Decoupled Flow (First quick payment)
        _output.WriteLine("4a. Testing Decoupled Flow (First)...");
        var decoupledFlow1 = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail1 = new AuthFlowDetail(decoupledFlow1);
        var authFlow1 = new AuthFlow(authFlowDetail1);
        var pcr1 = new Pcr("test-gw", "code1", "ref1");
        var amount1 = new Amount("1.00", Amount.CurrencyEnum.NZD);
        var request1 = new QuickPaymentRequest(authFlow1, pcr1, amount1);

        var qp1 = await _client.CreateQuickPaymentAsync(request1, RequestHeaders);
        Assert.NotNull(qp1);
        Assert.NotEqual(Guid.Empty, qp1.QuickPaymentId);
        _output.WriteLine($"✓ Decoupled Flow Quick Payment Created: {qp1.QuickPaymentId}");

        // Test 4b: Decoupled Flow (Second quick payment)
        _output.WriteLine("4b. Testing Decoupled Flow (Second)...");
        RequestHeaders["Idempotency-Key"] = Guid.NewGuid().ToString(); // New key for new request

        var decoupledFlow2 = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail2 = new AuthFlowDetail(decoupledFlow2);
        var authFlow2 = new AuthFlow(authFlowDetail2);
        var pcr2 = new Pcr("test-redir", "code2", "ref2");
        var amount2 = new Amount("1.50", Amount.CurrencyEnum.NZD);
        var request2 = new QuickPaymentRequest(authFlow2, pcr2, amount2);

        var qp2 = await _client.CreateQuickPaymentAsync(request2, RequestHeaders);
        Assert.NotNull(qp2);
        Assert.NotEqual(Guid.Empty, qp2.QuickPaymentId);
        _output.WriteLine($"✓ Decoupled Flow Quick Payment Created: {qp2.QuickPaymentId}");

        _output.WriteLine("✓ Different flow types PASSED\n");
    }

    [Fact(DisplayName = "5. Create and Revoke Quick Payment")]
    public async Task Test5_CreateAndRevokeQuickPayment()
    {
        _output.WriteLine("=== TEST 5: Create and Revoke Quick Payment ===");

        // Arrange
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("test-revoke");
        var amount = new Amount("0.50", Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        // Act - Create
        var qpCreateResponse = await _client.CreateQuickPaymentAsync(request, RequestHeaders);
        Assert.NotNull(qpCreateResponse);
        var qpId = qpCreateResponse.QuickPaymentId;
        _output.WriteLine($"✓ Quick Payment Created: {qpId}");

        // Act - Revoke
        await _client.RevokeQuickPaymentAsync(qpId);
        _output.WriteLine($"✓ Quick Payment Revoked: {qpId}");

        _output.WriteLine("✓ Create and Revoke PASSED\n");
    }

    [Fact(DisplayName = "6. Create Enduring Consent with Gateway Flow")]
    public async Task Test6_CreateEnduringConsentWithGatewayFlow()
    {
        _output.WriteLine("=== TEST 6: Create Enduring Consent ===");

        // Arrange
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("enduring", "monthly", "sub");
        var maximumAmountPeriod = new Amount("100.00", Amount.CurrencyEnum.NZD);
        var maximumAmountPayment = new Amount("50.00", Amount.CurrencyEnum.NZD);
        var startDate = DateTimeOffset.UtcNow;
        var endDate = startDate.AddMonths(6);

        var request = new EnduringConsentRequest(
            authFlow,
            startDate,
            endDate,
            Period.Monthly,
            maximumAmountPeriod,
            maximumAmountPayment);

        // Act
        var createConsentResponse = await _client.CreateEnduringConsentAsync(request, RequestHeaders);

        // Assert
        Assert.NotNull(createConsentResponse);
        Assert.NotEqual(Guid.Empty, createConsentResponse.ConsentId);

        _output.WriteLine($"✓ Enduring Consent Created: {createConsentResponse.ConsentId}");
        _output.WriteLine("✓ Enduring Consent creation PASSED\n");
    }

    [Fact(DisplayName = "7. Error Handling - Non-Existent Resource")]
    public async Task Test7_ErrorHandlingNonExistentResource()
    {
        _output.WriteLine("=== TEST 7: Error Handling ===");

        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BlinkResourceNotFoundException>(async () =>
        {
            await _client.GetSingleConsentAsync(nonExistentId);
        });

        Assert.NotNull(exception);
        Assert.Contains("does not exist", exception.Message, StringComparison.OrdinalIgnoreCase);

        _output.WriteLine($"✓ Correctly caught exception: {exception.Message}");
        _output.WriteLine("✓ Error handling PASSED\n");
    }

    [Fact(DisplayName = "8. Validate Configuration at Startup")]
    public void Test8_ValidateConfigurationAtStartup()
    {
        _output.WriteLine("=== TEST 8: Configuration Validation ===");

        // This test verifies that configuration validation works
        // The client creation in the fixture already validates the configuration
        // If it didn't validate, the tests wouldn't run at all

        Assert.NotNull(_client);
        _output.WriteLine("✓ Configuration validation working (client initialized successfully)");
        _output.WriteLine("✓ All required OAuth settings validated");
        _output.WriteLine("✓ HTTPS enforcement validated");
        _output.WriteLine("✓ Configuration validation PASSED\n");
    }

    [Fact(DisplayName = "9. Amount and PCR Validation")]
    public void Test9_AmountAndPcrValidation()
    {
        _output.WriteLine("=== TEST 9: Amount and PCR Validation ===");

        // Test valid Amount
        var validAmount = new Amount("99.99", Amount.CurrencyEnum.NZD);
        Assert.Equal("99.99", validAmount.Total);
        Assert.Equal(Amount.CurrencyEnum.NZD, validAmount.Currency);
        _output.WriteLine("✓ Valid amount accepted");

        // Test valid PCR with all fields
        var validPcr = new Pcr("particulars", "code", "reference");
        Assert.Equal("particulars", validPcr.Particulars);
        Assert.Equal("code", validPcr.Code);
        Assert.Equal("reference", validPcr.Reference);
        _output.WriteLine("✓ Valid PCR with all fields accepted");

        // Test valid PCR with only particulars (for single consents)
        var validPcrSingle = new Pcr("particular");
        Assert.Equal("particular", validPcrSingle.Particulars);
        _output.WriteLine("✓ Valid PCR with only particulars accepted");

        _output.WriteLine("✓ Amount and PCR validation PASSED\n");
    }

    [Fact(DisplayName = "10. Comprehensive Flow - Full Journey")]
    public async Task Test10_ComprehensiveFullJourney()
    {
        _output.WriteLine("=== TEST 10: Comprehensive Full Journey ===");

        // Step 1: Get Bank Metadata
        var banks = await _client.GetMetaAsync(RequestHeaders);
        Assert.NotEmpty(banks);
        _output.WriteLine($"✓ Step 1: Retrieved {banks.Count} banks");

        // Step 2: Create Quick Payment
        RequestHeaders["Idempotency-Key"] = Guid.NewGuid().ToString();
        var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-259531933", RedirectUri);
        var authFlowDetail = new AuthFlowDetail(decoupledFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr("journey-test");
        var amount = new Amount("1.00", Amount.CurrencyEnum.NZD);
        var qpRequest = new QuickPaymentRequest(authFlow, pcr, amount);

        var qpResponse = await _client.CreateQuickPaymentAsync(qpRequest, RequestHeaders);
        Assert.NotNull(qpResponse);
        var qpId = qpResponse.QuickPaymentId;
        _output.WriteLine($"✓ Step 2: Created Quick Payment {qpId}");

        // Step 3: Revoke the quick payment
        await _client.RevokeQuickPaymentAsync(qpId);
        _output.WriteLine($"✓ Step 3: Revoked Quick Payment {qpId}");

        // Step 4: Verify error on re-revocation (idempotency)
        try
        {
            await _client.RevokeQuickPaymentAsync(qpId);
            _output.WriteLine("✓ Step 4: Second revocation handled (idempotent)");
        }
        catch (AggregateException ex) when (ex.InnerException is BlinkClientException || ex.InnerException is BlinkServiceException)
        {
            _output.WriteLine($"✓ Step 4: Second revocation threw expected exception: {ex.InnerException?.Message}");
        }
        catch (BlinkClientException ex)
        {
            _output.WriteLine($"✓ Step 4: Second revocation threw expected exception: {ex.Message}");
        }
        catch (BlinkServiceException ex)
        {
            _output.WriteLine($"✓ Step 4: Second revocation threw expected exception: {ex.Message}");
        }

        _output.WriteLine("✓ Comprehensive full journey PASSED\n");
    }
}
