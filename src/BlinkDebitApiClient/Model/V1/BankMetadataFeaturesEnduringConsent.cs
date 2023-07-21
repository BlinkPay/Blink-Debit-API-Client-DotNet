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
/// The enduring consent bank feature
/// </summary>
[DataContract(Name = "bank_metadata_features_enduring_consent")]
public class BankMetadataFeaturesEnduringConsent : IEquatable<BankMetadataFeaturesEnduringConsent>,
    IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeaturesEnduringConsent" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected BankMetadataFeaturesEnduringConsent()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeaturesEnduringConsent" /> class.
    /// </summary>
    /// <param name="enabled">If enduring consent is disabled, only single payment consents can be issued. (required).</param>
    /// <param name="maximumConsent">ISO8601 time duration for the maximum allowed enduring consent period, i.e. how long the consent could be used to execute payments for. Appears only if consent_indefinite is false.</param>
    /// <param name="consentIndefinite">If the consenting period for payments is indefinite or time-limited by the bank.</param>
    public BankMetadataFeaturesEnduringConsent(bool enabled = default(bool),
        string maximumConsent = default(string), bool consentIndefinite = default(bool))
    {
        Enabled = enabled;
        MaximumConsent = maximumConsent;
        ConsentIndefinite = consentIndefinite;
    }

    /// <summary>
    /// If enduring consent is disabled, only single payment consents can be issued.
    /// </summary>
    /// <value>If enduring consent is disabled, only single payment consents can be issued.</value>
    [DataMember(Name = "enabled", IsRequired = true, EmitDefaultValue = true)]
    public bool Enabled { get; set; }

    /// <summary>
    /// ISO8601 time duration for the maximum allowed enduring consent period, i.e. how long the consent could be used to execute payments for. Appears only if consent_indefinite is false
    /// </summary>
    /// <value>ISO8601 time duration for the maximum allowed enduring consent period, i.e. how long the consent could be used to execute payments for. Appears only if consent_indefinite is false</value>
    /// <example>&quot;P12M0DT00H00M00S&quot;</example>
    [DataMember(Name = "maximum_consent", EmitDefaultValue = false)]
    public string MaximumConsent { get; set; }

    /// <summary>
    /// If the consenting period for payments is indefinite or time-limited by the bank
    /// </summary>
    /// <value>If the consenting period for payments is indefinite or time-limited by the bank</value>
    [DataMember(Name = "consent_indefinite", EmitDefaultValue = true)]
    public bool ConsentIndefinite { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class BankMetadataFeaturesEnduringConsent {\n");
        sb.Append("  Enabled: ").Append(Enabled).Append('\n');
        sb.Append("  MaximumConsent: ").Append(MaximumConsent).Append('\n');
        sb.Append("  ConsentIndefinite: ").Append(ConsentIndefinite).Append('\n');
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
        return Equals(input as BankMetadataFeaturesEnduringConsent);
    }

    /// <summary>
    /// Returns true if BankMetadataFeaturesEnduringConsent instances are equal
    /// </summary>
    /// <param name="input">Instance of BankMetadataFeaturesEnduringConsent to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BankMetadataFeaturesEnduringConsent input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Enabled == input.Enabled ||
                Enabled.Equals(input.Enabled)
            ) &&
            (
                MaximumConsent == input.MaximumConsent ||
                (MaximumConsent != null &&
                 MaximumConsent.Equals(input.MaximumConsent))
            ) &&
            (
                ConsentIndefinite == input.ConsentIndefinite ||
                ConsentIndefinite.Equals(input.ConsentIndefinite)
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
            hashCode = (hashCode * 59) + Enabled.GetHashCode();
            if (MaximumConsent != null)
            {
                hashCode = (hashCode * 59) + MaximumConsent.GetHashCode();
            }

            hashCode = (hashCode * 59) + ConsentIndefinite.GetHashCode();
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