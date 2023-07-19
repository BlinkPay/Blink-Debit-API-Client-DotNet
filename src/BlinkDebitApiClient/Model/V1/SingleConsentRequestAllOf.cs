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
/// The single consent detail
/// </summary>
[DataContract(Name = "single_consent_request_allOf")]
public class SingleConsentRequestAllOf : IEquatable<SingleConsentRequestAllOf>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleConsentRequestAllOf" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected SingleConsentRequestAllOf()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleConsentRequestAllOf" /> class.
    /// </summary>
    /// <param name="flow">flow (required).</param>
    /// <param name="pcr">pcr (required).</param>
    /// <param name="amount">amount (required).</param>
    public SingleConsentRequestAllOf(AuthFlow flow = default(AuthFlow), Pcr pcr = default(Pcr),
        Amount amount = default(Amount))
    {
        // to ensure "flow" is required (not null)
        Flow = flow ?? throw new BlinkInvalidValueException(
            "flow is a required property for SingleConsentRequestAllOf and cannot be null");
        // to ensure "pcr" is required (not null)
        Pcr = pcr ?? throw new BlinkInvalidValueException(
            "pcr is a required property for SingleConsentRequestAllOf and cannot be null");
        // to ensure "amount" is required (not null)
        Amount = amount ?? throw new BlinkInvalidValueException(
            "amount is a required property for SingleConsentRequestAllOf and cannot be null");
    }

    /// <summary>
    /// Gets or Sets Flow
    /// </summary>
    [DataMember(Name = "flow", IsRequired = true, EmitDefaultValue = true)]
    public AuthFlow Flow { get; set; }

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
        StringBuilder sb = new StringBuilder();
        sb.Append("class SingleConsentRequestAllOf {\n");
        sb.Append("  Flow: ").Append(Flow).Append('\n');
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
        return Equals(input as SingleConsentRequestAllOf);
    }

    /// <summary>
    /// Returns true if SingleConsentRequestAllOf instances are equal
    /// </summary>
    /// <param name="input">Instance of SingleConsentRequestAllOf to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(SingleConsentRequestAllOf input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Flow == input.Flow ||
                (Flow != null &&
                 Flow.Equals(input.Flow))
            ) &&
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
            int hashCode = 41;
            if (Flow != null)
            {
                hashCode = (hashCode * 59) + Flow.GetHashCode();
            }

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