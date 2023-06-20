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
/// The bank name. Required if not using Blink&#39;s hosted gateway.
/// </summary>
/// <value>The bank name. Required if not using Blink&#39;s hosted gateway.</value>
[JsonConverter(typeof(StringEnumConverter))]
public enum Bank
{
    /// <summary>
    /// Enum ASB for value: ASB
    /// </summary>
    [EnumMember(Value = "ASB")] ASB = 1,

    /// <summary>
    /// Enum ANZ for value: ANZ
    /// </summary>
    [EnumMember(Value = "ANZ")] ANZ = 2,

    /// <summary>
    /// Enum BNZ for value: BNZ
    /// </summary>
    [EnumMember(Value = "BNZ")] BNZ = 3,

    /// <summary>
    /// Enum Westpac for value: Westpac
    /// </summary>
    [EnumMember(Value = "Westpac")] Westpac = 4,

    /// <summary>
    /// Enum KiwiBank for value: KiwiBank
    /// </summary>
    [EnumMember(Value = "KiwiBank")] KiwiBank = 5,

    /// <summary>
    /// Enum PNZ for value: PNZ
    /// </summary>
    [EnumMember(Value = "PNZ")] PNZ = 6
}