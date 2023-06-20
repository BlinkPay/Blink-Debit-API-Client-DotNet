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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// Amount with currency.
/// </summary>
[DataContract(Name = "amount")]
public class Amount : IEquatable<Amount>, IValidatableObject
{
    /// <summary>
    /// The currency. Only NZD is supported.
    /// </summary>
    /// <value>The currency. Only NZD is supported.</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CurrencyEnum
    {
        /// <summary>
        /// Enum NZD for value: NZD
        /// </summary>
        [EnumMember(Value = "NZD")] NZD = 1
    }


    /// <summary>
    /// The currency. Only NZD is supported.
    /// </summary>
    /// <value>The currency. Only NZD is supported.</value>
    /// <example>&quot;NZD&quot;</example>
    [DataMember(Name = "currency", IsRequired = true, EmitDefaultValue = true)]
    public CurrencyEnum Currency { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Amount" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected Amount()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Amount" /> class.
    /// </summary>
    /// <param name="total">The amount. (required).</param>
    /// <param name="currency">The currency. Only NZD is supported. (required).</param>
    public Amount(string total = default(string), CurrencyEnum currency = default(CurrencyEnum))
    {
        // to ensure "total" is required (not null)
        Total = total ??
                throw new ArgumentNullException("total is a required property for Amount and cannot be null");
        Currency = currency;
    }

    /// <summary>
    /// The amount.
    /// </summary>
    /// <value>The amount.</value>
    /// <example>&quot;100.00&quot;</example>
    [DataMember(Name = "total", IsRequired = true, EmitDefaultValue = true)]
    public string Total { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Amount {\n");
        sb.Append("  Total: ").Append(Total).Append('\n');
        sb.Append("  Currency: ").Append(Currency).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as Amount);
    }

    /// <summary>
    /// Returns true if Amount instances are equal
    /// </summary>
    /// <param name="input">Instance of Amount to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Amount input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Total == input.Total ||
                (Total != null &&
                 Total.Equals(input.Total))
            ) &&
            (
                Currency == input.Currency ||
                Currency.Equals(input.Currency)
            );
    }

    /// <summary>
    /// Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 41;
            if (Total != null)
            {
                hashCode = (hashCode * 59) + Total.GetHashCode();
            }

            hashCode = (hashCode * 59) + Currency.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        // Total (string) pattern
        var regexTotal = new Regex(@"^\d{1,13}\.\d{1,2}$", RegexOptions.CultureInvariant);
        if (false == regexTotal.Match(Total).Success)
        {
            yield return new ValidationResult("Invalid value for Total, must match a pattern of " + regexTotal,
                new[] { "Total" });
        }
    }
}