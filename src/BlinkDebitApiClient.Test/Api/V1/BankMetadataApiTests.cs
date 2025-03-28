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
using BlinkDebitApiClient.Enums;
using BlinkDebitApiClient.Model.V1;
using Xunit;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
///  Class for testing BankMetadataApi
/// </summary>
[Collection("Blink Debit Collection")]
public class BankMetadataApiTests : IDisposable
{
    private static readonly Dictionary<string, string?> RequestHeaders = new Dictionary<string, string?>();

    private readonly BankMetadataApi _instance;

    public BankMetadataApiTests(BlinkDebitFixture fixture)
    {
        _instance = fixture.BankMetadataApi;

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
    /// Test an instance of BankMetadataApi
    /// </summary>
    [Fact]
    public void InstanceTest()
    {
        Assert.IsType<BankMetadataApi>(_instance);
    }

    /// <summary>
    /// Verify that bank metadata is retrieved
    /// </summary>
    [Fact(DisplayName = "Verify that bank metadata is retrieved")]
    public async void GetMeta()
    {
        var response = await _instance.GetMetaAsync(RequestHeaders);
        Assert.IsType<List<BankMetadata>>(response);
        Assert.Equal(6, response.Count);
    }
}