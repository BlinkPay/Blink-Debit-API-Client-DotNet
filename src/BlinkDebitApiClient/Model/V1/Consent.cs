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
using BlinkDebitApiClient.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The model for a consent.
/// </summary>
[DataContract(Name = "consent")]
public class Consent : IEquatable<Consent>, IValidatableObject
{
    /// <summary>
    /// The status of the consent
    /// </summary>
    /// <value>The status of the consent</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusEnum
    {
        /// <summary>
        /// Enum GatewayAwaitingSubmission for value: GatewayAwaitingSubmission
        /// </summary>
        [EnumMember(Value = "GatewayAwaitingSubmission")]
        GatewayAwaitingSubmission = 1,

        /// <summary>
        /// Enum GatewayTimeout for value: GatewayTimeout
        /// </summary>
        [EnumMember(Value = "GatewayTimeout")] GatewayTimeout = 2,

        /// <summary>
        /// Enum AwaitingAuthorisation for value: AwaitingAuthorisation
        /// </summary>
        [EnumMember(Value = "AwaitingAuthorisation")]
        AwaitingAuthorisation = 3,

        /// <summary>
        /// Enum Authorised for value: Authorised
        /// </summary>
        [EnumMember(Value = "Authorised")] Authorised = 4,

        /// <summary>
        /// Enum Consumed for value: Consumed
        /// </summary>
        [EnumMember(Value = "Consumed")] Consumed = 5,

        /// <summary>
        /// Enum Rejected for value: Rejected
        /// </summary>
        [EnumMember(Value = "Rejected")] Rejected = 6,

        /// <summary>
        /// Enum Revoked for value: Revoked
        /// </summary>
        [EnumMember(Value = "Revoked")] Revoked = 7
    }


    /// <summary>
    /// The status of the consent
    /// </summary>
    /// <value>The status of the consent</value>
    [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
    public StatusEnum Status { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Consent" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected Consent()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Consent" /> class.
    /// </summary>
    /// <param name="consentId">The consent ID (required).</param>
    /// <param name="status">The status of the consent (required).</param>
    /// <param name="creationTimestamp">The timestamp that the consent was created (required).</param>
    /// <param name="statusUpdatedTimestamp">The time that the status was last updated (required).</param>
    /// <param name="detail">detail (required).</param>
    /// <param name="payments">Payments associated with this consent, if any. (required).</param>
    public Consent(Guid consentId = default(Guid), StatusEnum status = default(StatusEnum),
        DateTimeOffset creationTimestamp = default(DateTimeOffset),
        DateTimeOffset statusUpdatedTimestamp = default(DateTimeOffset),
        ConsentDetail detail = default(ConsentDetail),
        List<Payment> payments = default(List<Payment>),
        CardNetwork cardNetwork = default(CardNetwork))
    {
        ConsentId = consentId;
        Status = status;
        CreationTimestamp = creationTimestamp;
        StatusUpdatedTimestamp = statusUpdatedTimestamp;
        // to ensure "detail" is required (not null)
        Detail = detail ??
                 throw new BlinkInvalidValueException("detail is a required property for Consent and cannot be null");
        // to ensure "payments" is required (not null)
        Payments = payments ??
                   throw new BlinkInvalidValueException(
                       "payments is a required property for Consent and cannot be null");
        CardNetwork = cardNetwork;
    }

    /// <summary>
    /// The consent ID
    /// </summary>
    /// <value>The consent ID</value>
    [DataMember(Name = "consent_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid ConsentId { get; set; }

    /// <summary>
    /// The timestamp that the consent was created
    /// </summary>
    /// <value>The timestamp that the consent was created</value>
    [DataMember(Name = "creation_timestamp", IsRequired = true, EmitDefaultValue = true)]
    public DateTimeOffset CreationTimestamp { get; set; }

    /// <summary>
    /// The time that the status was last updated
    /// </summary>
    /// <value>The time that the status was last updated</value>
    [DataMember(Name = "status_updated_timestamp", IsRequired = false, EmitDefaultValue = true)]
    public DateTimeOffset StatusUpdatedTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets Detail
    /// </summary>
    [DataMember(Name = "detail", IsRequired = true, EmitDefaultValue = true)]
    public ConsentDetail Detail { get; set; }

    /// <summary>
    /// Payments associated with this consent, if any.
    /// </summary>
    /// <value>Payments associated with this consent, if any.</value>
    [DataMember(Name = "payments", IsRequired = true, EmitDefaultValue = true)]
    public List<Payment> Payments { get; set; }

    /// <summary>
    /// Gets or Sets CardNetwork
    /// </summary>
    [DataMember(Name="card_network", EmitDefaultValue=false)]
    public CardNetwork CardNetwork { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Consent {\n");
        sb.Append("  ConsentId: ").Append(ConsentId).Append('\n');
        sb.Append("  Status: ").Append(Status).Append('\n');
        sb.Append("  CreationTimestamp: ").Append(CreationTimestamp).Append('\n');
        sb.Append("  StatusUpdatedTimestamp: ").Append(StatusUpdatedTimestamp).Append('\n');
        sb.Append("  Detail: ").Append(Detail).Append('\n');
        sb.Append("  Payments: ").Append(Payments).Append('\n');
        sb.Append("  CardNetwork: ").Append(CardNetwork).Append("\n");
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
        return Equals(input as Consent);
    }

    /// <summary>
    /// Returns true if Consent instances are equal
    /// </summary>
    /// <param name="input">Instance of Consent to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Consent input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                ConsentId == input.ConsentId ||
                (ConsentId != null &&
                 ConsentId.Equals(input.ConsentId))
            ) &&
            (
                Status == input.Status ||
                Status.Equals(input.Status)
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
                Payments == input.Payments ||
                Payments != null &&
                input.Payments != null &&
                Payments.SequenceEqual(input.Payments)
            ) && 
            (
                CardNetwork == input.CardNetwork ||
                (CardNetwork != null &&
                 CardNetwork.Equals(input.CardNetwork))
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
            if (ConsentId != null)
            {
                hashCode = (hashCode * 59) + ConsentId.GetHashCode();
            }

            hashCode = (hashCode * 59) + Status.GetHashCode();
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

            if (Payments != null)
            {
                hashCode = (hashCode * 59) + Payments.GetHashCode();
            }

            if (CardNetwork != null)
            {
                hashCode = (hashCode * 59) + CardNetwork.GetHashCode();
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