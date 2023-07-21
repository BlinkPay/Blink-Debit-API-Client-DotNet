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
/// The enduring consent detail
/// </summary>
[DataContract(Name = "enduring_consent_request_allOf")]
public class EnduringConsentRequestAllOf : IEquatable<EnduringConsentRequestAllOf>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets Period
    /// </summary>
    [DataMember(Name = "period", IsRequired = true, EmitDefaultValue = true)]
    public Period Period { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnduringConsentRequestAllOf" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected EnduringConsentRequestAllOf()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnduringConsentRequestAllOf" /> class.
    /// </summary>
    /// <param name="flow">flow (required).</param>
    /// <param name="fromTimestamp">The ISO 8601 start date to calculate the periods for which to calculate the consent period. (required).</param>
    /// <param name="expiryTimestamp">The ISO 8601 timeout for when an enduring consent will expire. If this field is blank, an indefinite request will be attempted..</param>
    /// <param name="period">period (required).</param>
    /// <param name="maximumAmountPeriod">maximumAmountPeriod (required).</param>
    public EnduringConsentRequestAllOf(AuthFlow flow = default(AuthFlow), DateTimeOffset fromTimestamp = default(DateTimeOffset),
        DateTimeOffset expiryTimestamp = default(DateTimeOffset), Period period = default(Period),
        Amount maximumAmountPeriod = default(Amount))
    {
        // to ensure "flow" is required (not null)
        Flow = flow ?? throw new BlinkInvalidValueException(
            "flow is a required property for EnduringConsentRequestAllOf and cannot be null");
        FromTimestamp = fromTimestamp;
        Period = period;
        // to ensure "maximumAmountPeriod" is required (not null)
        MaximumAmountPeriod = maximumAmountPeriod ?? throw new BlinkInvalidValueException(
            "maximumAmountPeriod is a required property for EnduringConsentRequestAllOf and cannot be null");
        ExpiryTimestamp = expiryTimestamp;
    }

    /// <summary>
    /// Gets or Sets Flow
    /// </summary>
    [DataMember(Name = "flow", IsRequired = true, EmitDefaultValue = true)]
    public AuthFlow Flow { get; set; }

    /// <summary>
    /// The ISO 8601 start date to calculate the periods for which to calculate the consent period.
    /// </summary>
    /// <value>The ISO 8601 start date to calculate the periods for which to calculate the consent period.</value>
    /// <example>&quot;2020-12-01T00:00+13:00&quot;</example>
    [DataMember(Name = "from_timestamp", IsRequired = true, EmitDefaultValue = true)]
    public DateTimeOffset FromTimestamp { get; set; }

    /// <summary>
    /// The ISO 8601 timeout for when an enduring consent will expire. If this field is blank, an indefinite request will be attempted.
    /// </summary>
    /// <value>The ISO 8601 timeout for when an enduring consent will expire. If this field is blank, an indefinite request will be attempted.</value>
    /// <example>&quot;2021-12-01T00:00+13:00&quot;</example>
    [DataMember(Name = "expiry_timestamp", EmitDefaultValue = false)]
    public DateTimeOffset ExpiryTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets MaximumAmountPeriod
    /// </summary>
    [DataMember(Name = "maximum_amount_period", IsRequired = true, EmitDefaultValue = true)]
    public Amount MaximumAmountPeriod { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class EnduringConsentRequestAllOf {\n");
        sb.Append("  Flow: ").Append(Flow).Append('\n');
        sb.Append("  FromTimestamp: ").Append(FromTimestamp).Append('\n');
        sb.Append("  ExpiryTimestamp: ").Append(ExpiryTimestamp).Append('\n');
        sb.Append("  Period: ").Append(Period).Append('\n');
        sb.Append("  MaximumAmountPeriod: ").Append(MaximumAmountPeriod).Append('\n');
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
        return Equals(input as EnduringConsentRequestAllOf);
    }

    /// <summary>
    /// Returns true if EnduringConsentRequestAllOf instances are equal
    /// </summary>
    /// <param name="input">Instance of EnduringConsentRequestAllOf to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EnduringConsentRequestAllOf input)
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
                FromTimestamp == input.FromTimestamp ||
                (FromTimestamp != null &&
                 FromTimestamp.Equals(input.FromTimestamp))
            ) &&
            (
                ExpiryTimestamp == input.ExpiryTimestamp ||
                (ExpiryTimestamp != null &&
                 ExpiryTimestamp.Equals(input.ExpiryTimestamp))
            ) &&
            (
                Period == input.Period ||
                Period.Equals(input.Period)
            ) &&
            (
                MaximumAmountPeriod == input.MaximumAmountPeriod ||
                (MaximumAmountPeriod != null &&
                 MaximumAmountPeriod.Equals(input.MaximumAmountPeriod))
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

            if (FromTimestamp != null)
            {
                hashCode = (hashCode * 59) + FromTimestamp.GetHashCode();
            }

            if (ExpiryTimestamp != null)
            {
                hashCode = (hashCode * 59) + ExpiryTimestamp.GetHashCode();
            }

            hashCode = (hashCode * 59) + Period.GetHashCode();
            if (MaximumAmountPeriod != null)
            {
                hashCode = (hashCode * 59) + MaximumAmountPeriod.GetHashCode();
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