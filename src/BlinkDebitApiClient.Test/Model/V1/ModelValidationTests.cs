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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BlinkDebitApiClient.Model.V1;
using Xunit;

namespace BlinkDebitApiClient.Test.Model.V1;

/// <summary>
/// Unit tests for the request models' <see cref="IValidatableObject"/> rules, which are the
/// SDK's client-side guard against sending malformed payment instructions to the API.
/// </summary>
public class ModelValidationTests
{
    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, true);

        return results;
    }

    [Theory(DisplayName = "A well-formed total is accepted")]
    [InlineData("1.25")]
    [InlineData("0.01")]
    [InlineData("999999999999.99")]
    public void AcceptsWellFormedTotal(string total)
    {
        Assert.Empty(Validate(new Amount(total, Amount.CurrencyEnum.NZD)));
    }

    [Theory(DisplayName = "A malformed total is rejected")]
    [InlineData("1")] // no decimal separator
    [InlineData("1.234")] // too many decimal places
    [InlineData("1.")] // no decimal places
    [InlineData("abc")]
    public void RejectsMalformedTotal(string total)
    {
        var results = Validate(new Amount(total, Amount.CurrencyEnum.NZD));

        Assert.Contains(results, result => result.MemberNames.Contains("Total"));
    }

    [Fact(DisplayName = "A single consent PCR with only particulars is accepted")]
    public void AcceptsParticularsOnlyPcr()
    {
        Assert.Empty(Validate(new Pcr("particulars")));
    }

    [Fact(DisplayName = "An enduring consent PCR with all three fields is accepted")]
    public void AcceptsFullyPopulatedPcr()
    {
        Assert.Empty(Validate(new Pcr("particulars", "code", "reference")));
    }

    [Theory(DisplayName = "An overlong PCR field is rejected")]
    [InlineData("particularsss", null, null, "Particulars")]
    [InlineData("particulars", "codecodecodec", null, "Code")]
    [InlineData("particulars", "code", "referencerefe", "Reference")]
    public void RejectsOverlongPcrField(string particulars, string? code, string? reference, string expectedMember)
    {
        var results = Validate(new Pcr(particulars, code, reference));

        Assert.Contains(results, result => result.MemberNames.Contains(expectedMember));
    }

    [Fact(DisplayName = "Particulars outside the permitted character set are rejected")]
    public void RejectsUnsupportedParticularsCharacters()
    {
        var results = Validate(new Pcr("€€€"));

        Assert.Contains(results, result => result.MemberNames.Contains("Particulars"));
    }

    [Fact(DisplayName = "A correctly formatted account number is accepted")]
    public void AcceptsWellFormedAccountNumber()
    {
        Assert.Empty(Validate(CreateRefund("00-0000-0000000-00")));
    }

    [Theory(DisplayName = "A malformed account number is rejected")]
    [InlineData("00-0000-0000000")] // missing the suffix
    [InlineData("0-0000-0000000-00")] // too few leading digits
    [InlineData("not-an-account-number")]
    public void RejectsMalformedAccountNumber(string accountNumber)
    {
        var results = Validate(CreateRefund(accountNumber));

        Assert.Contains(results, result => result.MemberNames.Contains("AccountNumber"));
    }

    private static Refund CreateRefund(string accountNumber)
    {
        return new Refund(Guid.NewGuid(), Refund.StatusEnum.Processing, DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow, accountNumber,
            new RefundRequest(new AccountNumberRefundRequest(Guid.NewGuid())));
    }
}
