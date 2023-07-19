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
using BlinkDebitApiClient.Exceptions;
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The partial refund details
/// </summary>
[DataContract(Name = "partial_refund_request_allOf")]
public class PartialRefundRequestAllOf : IEquatable<PartialRefundRequestAllOf>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartialRefundRequestAllOf" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected PartialRefundRequestAllOf()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartialRefundRequestAllOf" /> class.
    /// </summary>
    /// <param name="amount">amount (required).</param>
    /// <param name="pcr">pcr (required).</param>
    /// <param name="consentRedirect">The URI that the merchant will need to visit to authorise the refund payment from their bank, if applicable..</param>
    public PartialRefundRequestAllOf(Amount amount = default(Amount), Pcr pcr = default(Pcr),
        string consentRedirect = default(string))
    {
        // to ensure "amount" is required (not null)
        Amount = amount ?? throw new BlinkInvalidValueException(
            "amount is a required property for PartialRefundRequestAllOf and cannot be null");
        // to ensure "pcr" is required (not null)
        Pcr = pcr ?? throw new BlinkInvalidValueException(
            "pcr is a required property for PartialRefundRequestAllOf and cannot be null");
        ConsentRedirect = consentRedirect;
    }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name = "amount", IsRequired = true, EmitDefaultValue = true)]
    public Amount Amount { get; set; }

    /// <summary>
    /// Gets or Sets Pcr
    /// </summary>
    [DataMember(Name = "pcr", IsRequired = true, EmitDefaultValue = true)]
    public Pcr Pcr { get; set; }

    /// <summary>
    /// The URI that the merchant will need to visit to authorise the refund payment from their bank, if applicable.
    /// </summary>
    /// <value>The URI that the merchant will need to visit to authorise the refund payment from their bank, if applicable.</value>
    [DataMember(Name = "consent_redirect", EmitDefaultValue = false)]
    public string ConsentRedirect { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class PartialRefundRequestAllOf {\n");
        sb.Append("  Amount: ").Append(Amount).Append('\n');
        sb.Append("  Pcr: ").Append(Pcr).Append('\n');
        sb.Append("  ConsentRedirect: ").Append(ConsentRedirect).Append('\n');
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
        return Equals(input as PartialRefundRequestAllOf);
    }

    /// <summary>
    /// Returns true if PartialRefundRequestAllOf instances are equal
    /// </summary>
    /// <param name="input">Instance of PartialRefundRequestAllOf to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(PartialRefundRequestAllOf input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Amount == input.Amount ||
                (Amount != null &&
                 Amount.Equals(input.Amount))
            ) &&
            (
                Pcr == input.Pcr ||
                (Pcr != null &&
                 Pcr.Equals(input.Pcr))
            ) &&
            (
                ConsentRedirect == input.ConsentRedirect ||
                (ConsentRedirect != null &&
                 ConsentRedirect.Equals(input.ConsentRedirect))
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
            if (Amount != null)
            {
                hashCode = (hashCode * 59) + Amount.GetHashCode();
            }

            if (Pcr != null)
            {
                hashCode = (hashCode * 59) + Pcr.GetHashCode();
            }

            if (ConsentRedirect != null)
            {
                hashCode = (hashCode * 59) + ConsentRedirect.GetHashCode();
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