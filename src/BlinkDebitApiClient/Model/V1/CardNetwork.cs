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
/// The card network.
/// </summary>
/// <value>The card network.</value>
[JsonConverter(typeof(StringEnumConverter))]
public enum CardNetwork
{
    /// <summary>
    /// Enum VISA for value: VISA
    /// </summary>
    [EnumMember(Value = "VISA")] VISA = 1,

    /// <summary>
    /// Enum MASTERCARD for value: MASTERCARD
    /// </summary>
    [EnumMember(Value = "MASTERCARD")] MASTERCARD = 2,

    /// <summary>
    /// Enum AMEX for value: AMEX
    /// </summary>
    [EnumMember(Value = "AMEX")] AMEX = 3,

    /// <summary>
    /// Enum DISCOVER for value: DISCOVER
    /// </summary>
    [EnumMember(Value = "DISCOVER")] DISCOVER = 4,

    /// <summary>
    /// Enum DINERSCLUB for value: DINERSCLUB
    /// </summary>
    [EnumMember(Value = "DINERSCLUB")] DINERSCLUB = 5,

    /// <summary>
    /// Enum JCB for value: JCB
    /// </summary>
    [EnumMember(Value = "JCB")] JCB = 6
}