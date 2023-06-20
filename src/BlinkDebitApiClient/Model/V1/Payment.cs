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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The model for a payment.
/// </summary>
[DataContract(Name = "payment")]
public class Payment : IEquatable<Payment>, IValidatableObject
{
    /// <summary>
    /// The type of payment (single or enduring).
    /// </summary>
    /// <value>The type of payment (single or enduring).</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum
    {
        /// <summary>
        /// Enum Single for value: single
        /// </summary>
        [EnumMember(Value = "single")] Single = 1,

        /// <summary>
        /// Enum Enduring for value: enduring
        /// </summary>
        [EnumMember(Value = "enduring")] Enduring = 2
    }


    /// <summary>
    /// The type of payment (single or enduring).
    /// </summary>
    /// <value>The type of payment (single or enduring).</value>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }

    /// <summary>
    /// The status of the payment.
    /// </summary>
    /// <value>The status of the payment.</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusEnum
    {
        /// <summary>
        /// Enum Pending for value: Pending
        /// </summary>
        [EnumMember(Value = "Pending")] Pending = 1,

        /// <summary>
        /// Enum AcceptedSettlementInProcess for value: AcceptedSettlementInProcess
        /// </summary>
        [EnumMember(Value = "AcceptedSettlementInProcess")]
        AcceptedSettlementInProcess = 2,

        /// <summary>
        /// Enum AcceptedSettlementCompleted for value: AcceptedSettlementCompleted
        /// </summary>
        [EnumMember(Value = "AcceptedSettlementCompleted")]
        AcceptedSettlementCompleted = 3,

        /// <summary>
        /// Enum Rejected for value: Rejected
        /// </summary>
        [EnumMember(Value = "Rejected")] Rejected = 4
    }


    /// <summary>
    /// The status of the payment.
    /// </summary>
    /// <value>The status of the payment.</value>
    [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
    public StatusEnum Status { get; set; }

    /// <summary>
    /// The reason for &#x60;AcceptedSettlementCompleted&#x60;.
    /// </summary>
    /// <value>The reason for &#x60;AcceptedSettlementCompleted&#x60;.</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AcceptedReasonEnum
    {
        /// <summary>
        /// Enum SourceBankPaymentSent for value: source_bank_payment_sent
        /// </summary>
        [EnumMember(Value = "source_bank_payment_sent")]
        SourceBankPaymentSent = 1,

        /// <summary>
        /// Enum CardNetworkAccepted for value: card_network_accepted
        /// </summary>
        [EnumMember(Value = "card_network_accepted")]
        CardNetworkAccepted = 2
    }


    /// <summary>
    /// The reason for &#x60;AcceptedSettlementCompleted&#x60;.
    /// </summary>
    /// <value>The reason for &#x60;AcceptedSettlementCompleted&#x60;.</value>
    [DataMember(Name = "accepted_reason", EmitDefaultValue = false)]
    public AcceptedReasonEnum? AcceptedReason { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Payment" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected Payment()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Payment" /> class.
    /// </summary>
    /// <param name="paymentId">The payment ID (required).</param>
    /// <param name="type">The type of payment (single or enduring). (required).</param>
    /// <param name="status">The status of the payment. (required).</param>
    /// <param name="acceptedReason">The reason for &#x60;AcceptedSettlementCompleted&#x60;..</param>
    /// <param name="creationTimestamp">The timestamp that the payment was created. (required).</param>
    /// <param name="statusUpdatedTimestamp">The timestamp that the payment status was last updated. (required).</param>
    /// <param name="detail">detail (required).</param>
    /// <param name="refunds">Refunds that are related to this payment, if any. (required).</param>
    public Payment(Guid paymentId = default(Guid), TypeEnum type = default(TypeEnum),
        StatusEnum status = default(StatusEnum), AcceptedReasonEnum? acceptedReason = default(AcceptedReasonEnum?),
        DateTimeOffset creationTimestamp = default(DateTimeOffset), DateTimeOffset statusUpdatedTimestamp = default(DateTimeOffset),
        PaymentRequest detail = default(PaymentRequest), List<Refund> refunds = default(List<Refund>))
    {
        PaymentId = paymentId;
        Type = type;
        Status = status;
        CreationTimestamp = creationTimestamp;
        StatusUpdatedTimestamp = statusUpdatedTimestamp;
        // to ensure "detail" is required (not null)
        Detail = detail ??
                 throw new ArgumentNullException("detail is a required property for Payment and cannot be null");
        // to ensure "refunds" is required (not null)
        Refunds = refunds ??
                  throw new ArgumentNullException("refunds is a required property for Payment and cannot be null");
        AcceptedReason = acceptedReason;
    }

    /// <summary>
    /// The payment ID
    /// </summary>
    /// <value>The payment ID</value>
    [DataMember(Name = "payment_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// The timestamp that the payment was created.
    /// </summary>
    /// <value>The timestamp that the payment was created.</value>
    [DataMember(Name = "creation_timestamp", IsRequired = true, EmitDefaultValue = true)]
    public DateTimeOffset CreationTimestamp { get; set; }

    /// <summary>
    /// The timestamp that the payment status was last updated.
    /// </summary>
    /// <value>The timestamp that the payment status was last updated.</value>
    [DataMember(Name = "status_updated_timestamp", IsRequired = true, EmitDefaultValue = true)]
    public DateTimeOffset StatusUpdatedTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets Detail
    /// </summary>
    [DataMember(Name = "detail", IsRequired = true, EmitDefaultValue = true)]
    public PaymentRequest Detail { get; set; }

    /// <summary>
    /// Refunds that are related to this payment, if any.
    /// </summary>
    /// <value>Refunds that are related to this payment, if any.</value>
    [DataMember(Name = "refunds", IsRequired = true, EmitDefaultValue = true)]
    public List<Refund> Refunds { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Payment {\n");
        sb.Append("  PaymentId: ").Append(PaymentId).Append('\n');
        sb.Append("  Type: ").Append(Type).Append('\n');
        sb.Append("  Status: ").Append(Status).Append('\n');
        sb.Append("  AcceptedReason: ").Append(AcceptedReason).Append('\n');
        sb.Append("  CreationTimestamp: ").Append(CreationTimestamp).Append('\n');
        sb.Append("  StatusUpdatedTimestamp: ").Append(StatusUpdatedTimestamp).Append('\n');
        sb.Append("  Detail: ").Append(Detail).Append('\n');
        sb.Append("  Refunds: ").Append(Refunds).Append('\n');
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
        return Equals(input as Payment);
    }

    /// <summary>
    /// Returns true if Payment instances are equal
    /// </summary>
    /// <param name="input">Instance of Payment to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Payment input)
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
                Status == input.Status ||
                Status.Equals(input.Status)
            ) &&
            (
                AcceptedReason == input.AcceptedReason ||
                AcceptedReason.Equals(input.AcceptedReason)
            ) &&
            (
                CreationTimestamp == input.CreationTimestamp ||
                (CreationTimestamp != null &&
                 CreationTimestamp.Equals(input.CreationTimestamp))
            ) &&
            (
                StatusUpdatedTimestamp == input.StatusUpdatedTimestamp ||
                (StatusUpdatedTimestamp != null &&
                 StatusUpdatedTimestamp.Equals(input.StatusUpdatedTimestamp))
            ) &&
            (
                Detail == input.Detail ||
                (Detail != null &&
                 Detail.Equals(input.Detail))
            ) &&
            (
                Refunds == input.Refunds ||
                Refunds != null &&
                input.Refunds != null &&
                Refunds.SequenceEqual(input.Refunds)
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
            hashCode = (hashCode * 59) + Status.GetHashCode();
            hashCode = (hashCode * 59) + AcceptedReason.GetHashCode();
            if (CreationTimestamp != null)
            {
                hashCode = (hashCode * 59) + CreationTimestamp.GetHashCode();
            }

            if (StatusUpdatedTimestamp != null)
            {
                hashCode = (hashCode * 59) + StatusUpdatedTimestamp.GetHashCode();
            }

            if (Detail != null)
            {
                hashCode = (hashCode * 59) + Detail.GetHashCode();
            }

            if (Refunds != null)
            {
                hashCode = (hashCode * 59) + Refunds.GetHashCode();
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