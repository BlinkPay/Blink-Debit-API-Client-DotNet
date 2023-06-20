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
/// The available bank features
/// </summary>
[DataContract(Name = "bank_metadata_features")]
public class BankMetadataFeatures : IEquatable<BankMetadataFeatures>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeatures" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected BankMetadataFeatures()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeatures" /> class.
    /// </summary>
    /// <param name="enduringConsent">enduringConsent (required).</param>
    /// <param name="decoupledFlow">decoupledFlow (required).</param>
    public BankMetadataFeatures(
        BankMetadataFeaturesEnduringConsent enduringConsent = default(BankMetadataFeaturesEnduringConsent),
        BankMetadataFeaturesDecoupledFlow decoupledFlow = default(BankMetadataFeaturesDecoupledFlow))
    {
        // to ensure "enduringConsent" is required (not null)
        EnduringConsent = enduringConsent ??
                          throw new ArgumentNullException(
                              "enduringConsent is a required property for BankMetadataFeatures and cannot be null");
        // to ensure "decoupledFlow" is required (not null)
        DecoupledFlow = decoupledFlow ??
                        throw new ArgumentNullException(
                            "decoupledFlow is a required property for BankMetadataFeatures and cannot be null");
    }

    /// <summary>
    /// Gets or Sets EnduringConsent
    /// </summary>
    [DataMember(Name = "enduring_consent", IsRequired = false, EmitDefaultValue = true)]
    public BankMetadataFeaturesEnduringConsent EnduringConsent { get; set; }

    /// <summary>
    /// Gets or Sets DecoupledFlow
    /// </summary>
    [DataMember(Name = "decoupled_flow", IsRequired = false, EmitDefaultValue = true)]
    public BankMetadataFeaturesDecoupledFlow DecoupledFlow { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class BankMetadataFeatures {\n");
        sb.Append("  EnduringConsent: ").Append(EnduringConsent).Append('\n');
        sb.Append("  DecoupledFlow: ").Append(DecoupledFlow).Append('\n');
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
        return Equals(input as BankMetadataFeatures);
    }

    /// <summary>
    /// Returns true if BankMetadataFeatures instances are equal
    /// </summary>
    /// <param name="input">Instance of BankMetadataFeatures to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BankMetadataFeatures input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                EnduringConsent == input.EnduringConsent ||
                (EnduringConsent != null &&
                 EnduringConsent.Equals(input.EnduringConsent))
            ) &&
            (
                DecoupledFlow == input.DecoupledFlow ||
                (DecoupledFlow != null &&
                 DecoupledFlow.Equals(input.DecoupledFlow))
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
            if (EnduringConsent != null)
            {
                hashCode = (hashCode * 59) + EnduringConsent.GetHashCode();
            }

            if (DecoupledFlow != null)
            {
                hashCode = (hashCode * 59) + DecoupledFlow.GetHashCode();
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