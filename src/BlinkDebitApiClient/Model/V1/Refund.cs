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
/// The model for a refund.
/// </summary>
[DataContract(Name = "refund")]
public class Refund : IEquatable<Refund>, IValidatableObject
{
    /// <summary>
    /// The refund status
    /// </summary>
    /// <value>The refund status</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusEnum
    {
        /// <summary>
        /// Enum Failed for value: failed
        /// </summary>
        [EnumMember(Value = "failed")] Failed = 1,

        /// <summary>
        /// Enum Processing for value: processing
        /// </summary>
        [EnumMember(Value = "processing")] Processing = 2,

        /// <summary>
        /// Enum Completed for value: completed
        /// </summary>
        [EnumMember(Value = "completed")] Completed = 3
    }


    /// <summary>
    /// The refund status
    /// </summary>
    /// <value>The refund status</value>
    [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
    public StatusEnum Status { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Refund" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected Refund()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Refund" /> class.
    /// </summary>
    /// <param name="refundId">The refund ID. (required).</param>
    /// <param name="status">The refund status (required).</param>
    /// <param name="creationTimestamp">The time that the refund was created. (required).</param>
    /// <param name="statusUpdatedTimestamp">The time that the status was last updated. (required).</param>
    /// <param name="accountNumber">The customer account number used or to be used for the refund. (required).</param>
    /// <param name="detail">detail (required).</param>
    public Refund(Guid refundId = default(Guid), StatusEnum status = default(StatusEnum),
        DateTimeOffset creationTimestamp = default(DateTimeOffset), DateTimeOffset statusUpdatedTimestamp = default(DateTimeOffset),
        string accountNumber = default(string), RefundRequest detail = default(RefundRequest))
    {
        RefundId = refundId;
        Status = status;
        CreationTimestamp = creationTimestamp;
        StatusUpdatedTimestamp = statusUpdatedTimestamp;
        // to ensure "accountNumber" is required (not null)
        AccountNumber = accountNumber ??
                        throw new ArgumentNullException(
                            "accountNumber is a required property for Refund and cannot be null");
        // to ensure "detail" is required (not null)
        Detail = detail ??
                 throw new ArgumentNullException("detail is a required property for Refund and cannot be null");
    }

    /// <summary>
    /// The refund ID.
    /// </summary>
    /// <value>The refund ID.</value>
    [DataMember(Name = "refund_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid RefundId { get; set; }

    /// <summary>
    /// The time that the refund was created.
    /// </summary>
    /// <value>The time that the refund was created.</value>
    [DataMember(Name = "creation_timestamp", IsRequired = true, EmitDefaultValue = true)]
    public DateTimeOffset CreationTimestamp { get; set; }

    /// <summary>
    /// The time that the status was last updated.
    /// </summary>
    /// <value>The time that the status was last updated.</value>
    [DataMember(Name = "status_updated_timestamp", IsRequired = true, EmitDefaultValue = true)]
    public DateTimeOffset StatusUpdatedTimestamp { get; set; }

    /// <summary>
    /// The customer account number used or to be used for the refund.
    /// </summary>
    /// <value>The customer account number used or to be used for the refund.</value>
    /// <example>&quot;00-0000-0000000-00&quot;</example>
    [DataMember(Name = "account_number", IsRequired = true, EmitDefaultValue = true)]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or Sets Detail
    /// </summary>
    [DataMember(Name = "detail", IsRequired = true, EmitDefaultValue = true)]
    public RefundRequest Detail { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Refund {\n");
        sb.Append("  RefundId: ").Append(RefundId).Append('\n');
        sb.Append("  Status: ").Append(Status).Append('\n');
        sb.Append("  CreationTimestamp: ").Append(CreationTimestamp).Append('\n');
        sb.Append("  StatusUpdatedTimestamp: ").Append(StatusUpdatedTimestamp).Append('\n');
        sb.Append("  AccountNumber: ").Append(AccountNumber).Append('\n');
        sb.Append("  Detail: ").Append(Detail).Append('\n');
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
        return Equals(input as Refund);
    }

    /// <summary>
    /// Returns true if Refund instances are equal
    /// </summary>
    /// <param name="input">Instance of Refund to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Refund input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                RefundId == input.RefundId ||
                (RefundId != null &&
                 RefundId.Equals(input.RefundId))
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
                AccountNumber == input.AccountNumber ||
                (AccountNumber != null &&
                 AccountNumber.Equals(input.AccountNumber))
            ) &&
            (
                Detail == input.Detail ||
                (Detail != null &&
                 Detail.Equals(input.Detail))
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
            if (RefundId != null)
            {
                hashCode = (hashCode * 59) + RefundId.GetHashCode();
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

            if (AccountNumber != null)
            {
                hashCode = (hashCode * 59) + AccountNumber.GetHashCode();
            }

            if (Detail != null)
            {
                hashCode = (hashCode * 59) + Detail.GetHashCode();
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
        // AccountNumber (string) pattern
        var regexAccountNumber = new Regex(@"^\d{2}-\d{4}-\d{7}-\d{2}$", RegexOptions.CultureInvariant);
        if (false == regexAccountNumber.Match(AccountNumber).Success)
        {
            yield return new ValidationResult(
                "Invalid value for AccountNumber, must match a pattern of " + regexAccountNumber,
                new[] { "AccountNumber" });
        }
    }
}