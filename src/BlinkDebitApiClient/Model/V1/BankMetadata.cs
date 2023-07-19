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
/// Information about a banks enabled features.
/// </summary>
[DataContract(Name = "bank-metadata")]
public class BankMetadata : IEquatable<BankMetadata>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public Bank Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadata" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected BankMetadata()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadata" /> class.
    /// </summary>
    /// <param name="name">name (required).</param>
    /// <param name="features">features (required).</param>
    /// <param name="redirectFlow">redirectFlow (required).</param>
    public BankMetadata(Bank name = default(Bank), BankMetadataFeatures features = default(BankMetadataFeatures),
        BankMetadataRedirectFlow redirectFlow = default(BankMetadataRedirectFlow))
    {
        Name = name;
        // to ensure "features" is required (not null)
        Features = features ??
                   throw new BlinkInvalidValueException(
                       "features is a required property for BankMetadata and cannot be null");
        // to ensure "redirectFlow" is required (not null)
        RedirectFlow = redirectFlow ??
                       throw new BlinkInvalidValueException(
                           "redirectFlow is a required property for BankMetadata and cannot be null");
    }

    /// <summary>
    /// Gets or Sets Features
    /// </summary>
    [DataMember(Name = "features", IsRequired = true, EmitDefaultValue = true)]
    public BankMetadataFeatures Features { get; set; }

    /// <summary>
    /// Gets or Sets RedirectFlow
    /// </summary>
    [DataMember(Name = "redirect_flow", IsRequired = true, EmitDefaultValue = true)]
    public BankMetadataRedirectFlow RedirectFlow { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class BankMetadata {\n");
        sb.Append("  Name: ").Append(Name).Append('\n');
        sb.Append("  Features: ").Append(Features).Append('\n');
        sb.Append("  RedirectFlow: ").Append(RedirectFlow).Append('\n');
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
        return Equals(input as BankMetadata);
    }

    /// <summary>
    /// Returns true if BankMetadata instances are equal
    /// </summary>
    /// <param name="input">Instance of BankMetadata to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BankMetadata input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Name == input.Name ||
                Name.Equals(input.Name)
            ) &&
            (
                Features == input.Features ||
                (Features != null &&
                 Features.Equals(input.Features))
            ) &&
            (
                RedirectFlow == input.RedirectFlow ||
                (RedirectFlow != null &&
                 RedirectFlow.Equals(input.RedirectFlow))
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
            hashCode = (hashCode * 59) + Name.GetHashCode();
            if (Features != null)
            {
                hashCode = (hashCode * 59) + Features.GetHashCode();
            }

            if (RedirectFlow != null)
            {
                hashCode = (hashCode * 59) + RedirectFlow.GetHashCode();
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