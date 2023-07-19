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
/// The PCR and to use in the &#x60;full_refund&#x60; request.
/// </summary>
[DataContract(Name = "full_refund")]
public class FullRefundRequest : RefundDetail, IEquatable<FullRefundRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FullRefundRequest" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected FullRefundRequest()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FullRefundRequest" /> class.
    /// </summary>
    /// <param name="paymentId">The payment ID. The payment must have a status of &#x60;AcceptedSettlementCompleted&#x60;. (required).</param>
    /// <param name="pcr">pcr (required).</param>
    /// <param name="consentRedirect">The URI that the merchant will need to visit to authorise the refund payment from their bank, if applicable..</param>
    /// <param name="type">The refund type. (required).</param>
    public FullRefundRequest(Guid paymentId = default(Guid), Pcr pcr = default(Pcr),
        string consentRedirect = default(string), TypeEnum type = TypeEnum.FullRefund)
    {
        PaymentId = paymentId;
        Type = type;
        // to ensure "pcr" is required (not null)
        Pcr = pcr ??
              throw new BlinkInvalidValueException(
                  "pcr is a required property for FullRefundRequest and cannot be null");
        ConsentRedirect = consentRedirect;
    }

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
        sb.Append("class FullRefundRequest {\n");
        sb.Append("  PaymentId: ").Append(PaymentId).Append('\n');
        sb.Append("  Type: ").Append(Type).Append('\n');
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
        return Equals(input as FullRefundRequest);
    }

    /// <summary>
    /// Returns true if FullRefundRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of FullRefundRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(FullRefundRequest input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                PaymentId == input.PaymentId ||
                (PaymentId != null &&
                 PaymentId.Equals(input.PaymentId))
            ) &&
            (
                Type == input.Type ||
                Type.Equals(input.Type)
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
            if (PaymentId != null)
            {
                hashCode = (hashCode * 59) + PaymentId.GetHashCode();
            }

            hashCode = (hashCode * 59) + Type.GetHashCode();
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