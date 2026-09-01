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
using System.Threading.Tasks;
using BlinkDebitApiClient.Api.V1;
using BlinkDebitApiClient.Config;
using BlinkDebitApiClient.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;
using RestSharp;
using Xunit;

namespace BlinkDebitApiClient.Test.Api.V1;

/// <summary>
/// Unit tests for the Polly retry policies that <see cref="BlinkDebitClient"/> installs.
/// <para>
/// <see cref="RetryConfiguration"/> holds the policies in process-wide static state, and
/// <see cref="ApiClient"/> switches to its retry branch for every request once they are set.
/// Installing them therefore changes how every other test in the assembly executes, so this
/// class snapshots the policies on construction and restores them in <see cref="Dispose"/>.
/// Joining the integration collection additionally stops other tests running concurrently
/// while the policies are swapped in; the collection alone would not be enough, because
/// xUnit does not guarantee the order of classes within it.
/// </para>
/// </summary>
[Collection("Blink Debit Collection")]
public class RetryPolicyTests : IDisposable
{
    private readonly Policy<RestResponse> _originalRetryPolicy;

    private readonly AsyncPolicy<RestResponse> _originalAsyncRetryPolicy;

    public RetryPolicyTests()
    {
        _originalRetryPolicy = RetryConfiguration.RetryPolicy;
        _originalAsyncRetryPolicy = RetryConfiguration.AsyncRetryPolicy;

        // ConfigureRetry is private, so constructing the client is the only way to exercise the
        // real policy configuration rather than a copy of it. The constructor validates its
        // arguments, builds an ApiClient and its OAuth authenticator, and installs the policies;
        // it issues no request, so these placeholder credentials are never sent anywhere.
        _ = new BlinkDebitClient(
            LoggerFactory.Create(builder => builder.AddDebug()).CreateLogger<RetryPolicyTests>(),
            "https://sandbox.debit.blinkpay.co.nz", "test-client-id", "test-client-secret", 10000, true);
    }

    /// <summary>
    /// Restores the policies installed before this class ran, so that the integration tests
    /// sharing this collection keep the retry behaviour their own fixture configured.
    /// </summary>
    public void Dispose()
    {
        RetryConfiguration.RetryPolicy = _originalRetryPolicy;
        RetryConfiguration.AsyncRetryPolicy = _originalAsyncRetryPolicy;
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "The synchronous policy retries a retryable failure and then succeeds")]
    public void SynchronousPolicyRetriesRetryableFailure()
    {
        var attempts = 0;

        var response = RetryConfiguration.RetryPolicy.Execute(() =>
        {
            if (++attempts == 1)
            {
                throw new BlinkRetryableException();
            }

            return new RestResponse(new RestRequest());
        });

        Assert.Equal(2, attempts);
        Assert.NotNull(response);
    }

    [Fact(DisplayName = "The asynchronous policy retries a retryable failure and then succeeds")]
    public async Task AsynchronousPolicyRetriesRetryableFailure()
    {
        var attempts = 0;

        var response = await RetryConfiguration.AsyncRetryPolicy.ExecuteAsync(() =>
        {
            if (++attempts == 1)
            {
                throw new BlinkRetryableException();
            }

            return Task.FromResult(new RestResponse(new RestRequest()));
        });

        Assert.Equal(2, attempts);
        Assert.NotNull(response);
    }
}
