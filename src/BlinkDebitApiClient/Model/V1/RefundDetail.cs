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
using Newtonsoft.Json.Converters;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The refund detail model.
/// </summary>
[DataContract(Name = "refund-detail")]
public class RefundDetail : IEquatable<RefundDetail>, IValidatableObject
{
    /// <summary>
    /// The refund type.
    /// </summary>
    /// <value>The refund type.</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum
    {
        /// <summary>
        /// Enum AccountNumber for value: account_number
        /// </summary>
        [EnumMember(Value = "account_number")] AccountNumber,

        /// <summary>
        /// Enum PartialRefund for value: partial_refund
        /// </summary>
        [EnumMember(Value = "partial_refund")] PartialRefund,

        /// <summary>
        /// Enum FullRefund for value: full_refund
        /// </summary>
        [EnumMember(Value = "full_refund")] FullRefund
    }


    /// <summary>
    /// The refund type.
    /// </summary>
    /// <value>The refund type.</value>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefundDetail" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected RefundDetail()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefundDetail" /> class.
    /// </summary>
    /// <param name="paymentId">The payment ID. The payment must have a status of &#x60;AcceptedSettlementCompleted&#x60;. (required).</param>
    /// <param name="type">The refund type. (required).</param>
    public RefundDetail(Guid paymentId = default(Guid), TypeEnum type = default(TypeEnum))
    {
        PaymentId = paymentId;
        Type = type;
    }

    /// <summary>
    /// The payment ID. The payment must have a status of &#x60;AcceptedSettlementCompleted&#x60;.
    /// </summary>
    /// <value>The payment ID. The payment must have a status of &#x60;AcceptedSettlementCompleted&#x60;.</value>
    [DataMember(Name = "payment_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class RefundDetail {\n");
        sb.Append("  PaymentId: ").Append(PaymentId).Append('\n');
        sb.Append("  Type: ").Append(Type).Append('\n');
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
        return Equals(input as RefundDetail);
    }

    /// <summary>
    /// Returns true if RefundDetail instances are equal
    /// </summary>
    /// <param name="input">Instance of RefundDetail to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RefundDetail input)
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