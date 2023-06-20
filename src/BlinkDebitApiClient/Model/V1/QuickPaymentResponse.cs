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
/// The model for quick payment response.
/// </summary>
[DataContract(Name = "quick-payment-response")]
public class QuickPaymentResponse : IEquatable<QuickPaymentResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuickPaymentResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected QuickPaymentResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuickPaymentResponse" /> class.
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID. (required).</param>
    /// <param name="consent">consent (required).</param>
    public QuickPaymentResponse(Guid quickPaymentId = default(Guid), Consent consent = default(Consent))
    {
        QuickPaymentId = quickPaymentId;
        // to ensure "consent" is required (not null)
        Consent = consent ?? throw new ArgumentNullException(
            "consent is a required property for QuickPaymentResponse and cannot be null");
    }

    /// <summary>
    /// The quick payment ID.
    /// </summary>
    /// <value>The quick payment ID.</value>
    [DataMember(Name = "quick_payment_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid QuickPaymentId { get; set; }

    /// <summary>
    /// Gets or Sets Consent
    /// </summary>
    [DataMember(Name = "consent", IsRequired = true, EmitDefaultValue = true)]
    public Consent Consent { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class QuickPaymentResponse {\n");
        sb.Append("  QuickPaymentId: ").Append(QuickPaymentId).Append('\n');
        sb.Append("  Consent: ").Append(Consent).Append('\n');
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
        return Equals(input as QuickPaymentResponse);
    }

    /// <summary>
    /// Returns true if QuickPaymentResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of QuickPaymentResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(QuickPaymentResponse input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                QuickPaymentId == input.QuickPaymentId ||
                (QuickPaymentId != null &&
                 QuickPaymentId.Equals(input.QuickPaymentId))
            ) &&
            (
                Consent == input.Consent ||
                (Consent != null &&
                 Consent.Equals(input.Consent))
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
            if (QuickPaymentId != null)
            {
                hashCode = (hashCode * 59) + QuickPaymentId.GetHashCode();
            }

            if (Consent != null)
            {
                hashCode = (hashCode * 59) + Consent.GetHashCode();
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