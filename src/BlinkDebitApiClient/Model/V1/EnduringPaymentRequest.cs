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
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The model for an enduring payment request, if applicable.
/// </summary>
[DataContract(Name = "enduring-payment-request")]
public class EnduringPaymentRequest : IEquatable<EnduringPaymentRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnduringPaymentRequest" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected EnduringPaymentRequest()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnduringPaymentRequest" /> class.
    /// </summary>
    /// <param name="pcr">pcr (required).</param>
    /// <param name="amount">amount (required).</param>
    public EnduringPaymentRequest(Pcr pcr = default(Pcr), Amount amount = default(Amount))
    {
        // to ensure "pcr" is required (not null)
        Pcr = pcr ??
              throw new ArgumentNullException(
                  "pcr is a required property for EnduringPaymentRequest and cannot be null");
        // to ensure "amount" is required (not null)
        Amount = amount ??
                 throw new ArgumentNullException(
                     "amount is a required property for EnduringPaymentRequest and cannot be null");
    }

    /// <summary>
    /// Gets or Sets Pcr
    /// </summary>
    [DataMember(Name = "pcr", IsRequired = true, EmitDefaultValue = true)]
    public Pcr Pcr { get; set; }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name = "amount", IsRequired = true, EmitDefaultValue = true)]
    public Amount Amount { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class EnduringPaymentRequest {\n");
        sb.Append("  Pcr: ").Append(Pcr).Append('\n');
        sb.Append("  Amount: ").Append(Amount).Append('\n');
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
        return Equals(input as EnduringPaymentRequest);
    }

    /// <summary>
    /// Returns true if EnduringPaymentRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of EnduringPaymentRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EnduringPaymentRequest input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Pcr == input.Pcr ||
                (Pcr != null &&
                 Pcr.Equals(input.Pcr))
            ) &&
            (
                Amount == input.Amount ||
                (Amount != null &&
                 Amount.Equals(input.Amount))
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
            if (Pcr != null)
            {
                hashCode = (hashCode * 59) + Pcr.GetHashCode();
            }

            if (Amount != null)
            {
                hashCode = (hashCode * 59) + Amount.GetHashCode();
            }

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
        yield break;
    }
}