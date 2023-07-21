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
/// The value type used to identify the customer with their bank.
/// </summary>
/// <value>The value type used to identify the customer with their bank.</value>
[JsonConverter(typeof(StringEnumConverter))]
public enum IdentifierType
{
    /// <summary>
    /// Enum Email for value: email
    /// </summary>
    [EnumMember(Value = "email")] Email = 1,

    /// <summary>
    /// Enum PhoneNumber for value: phone_number
    /// </summary>
    [EnumMember(Value = "phone_number")] PhoneNumber = 2,

    /// <summary>
    /// Enum MobileNumber for value: mobile_number
    /// </summary>
    [EnumMember(Value = "mobile_number")] MobileNumber = 3,

    /// <summary>
    /// Enum BankingUsername for value: banking_username
    /// </summary>
    [EnumMember(Value = "banking_username")]
    BankingUsername = 4,

    /// <summary>
    /// Enum ConsentId for value: consent_id
    /// </summary>
    [EnumMember(Value = "consent_id")] ConsentId = 5
}