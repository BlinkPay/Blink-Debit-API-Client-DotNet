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

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The type of card payment.
/// </summary>
/// <value>The type of card payment.</value>
[JsonConverter(typeof(StringEnumConverter))]
public enum CardPaymentType
{
    /// <summary>
    /// Enum APPLEPAY for value: APPLEPAY
    /// </summary>
    [EnumMember(Value = "APPLEPAY")] APPLEPAY = 1,

    /// <summary>
    /// Enum CLICKTOPAY for value: CLICKTOPAY
    /// </summary>
    [EnumMember(Value = "CLICKTOPAY")] CLICKTOPAY = 2,

    /// <summary>
    /// Enum GOOGLEPAY for value: GOOGLEPAY
    /// </summary>
    [EnumMember(Value = "GOOGLEPAY")] GOOGLEPAY = 3,

    /// <summary>
    /// Enum PANENTRY for value: PANENTRY
    /// </summary>
    [EnumMember(Value = "PANENTRY")] PANENTRY = 4
}