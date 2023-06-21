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

namespace BlinkDebitApiClient.Config;

/// <summary>
/// The Blink Pay properties for Blink Bills and Blink Debit (Blink AutoPay and Blink PayNow).
/// </summary>
public class BlinkPayProperties
{
    /// <summary>
    /// The Blink Debit hostname
    /// </summary>
    public string DebitUrl { get; set; } = "https://sandbox.debit.blinkpay.co.nz";

    /// <summary>
    /// The request timeout
    /// </summary>
    public string Timeout { get; set; } = "10000";

    /// <summary>
    /// The flag for retries
    /// </summary>
    public bool RetryEnabled { get; set; }

    /// <summary>
    /// The OAuth2 client ID
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// The OAuth2 client secret
    /// </summary>
    public string ClientSecret { get; set; }
}