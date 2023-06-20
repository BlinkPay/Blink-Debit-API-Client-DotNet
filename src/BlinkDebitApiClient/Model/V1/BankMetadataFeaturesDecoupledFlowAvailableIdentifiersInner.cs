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
/// BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner
/// </summary>
[DataContract(Name = "bank_metadata_features_decoupled_flow_available_identifiers_inner")]
public class BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner :
    IEquatable<BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public IdentifierType Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner" /> class.
    /// </summary>
    /// <param name="type">type (required).</param>
    /// <param name="regex">A regex that can be used for validation of the field.</param>
    /// <param name="name">The common name of the field (required).</param>
    /// <param name="description">The description of the field and/or where to find it.</param>
    public BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner(IdentifierType type = default(IdentifierType),
        string regex = default(string), string name = default(string), string description = default(string))
    {
        Type = type;
        // to ensure "name" is required (not null)
        Name = name ?? throw new ArgumentNullException(
            "name is a required property for BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner and cannot be null");
        Regex = regex;
        Description = description;
    }

    /// <summary>
    /// A regex that can be used for validation of the field
    /// </summary>
    /// <value>A regex that can be used for validation of the field</value>
    /// <example>&quot;^[0-9]{9}$&quot;</example>
    [DataMember(Name = "regex", EmitDefaultValue = false)]
    public string Regex { get; set; }

    /// <summary>
    /// The common name of the field
    /// </summary>
    /// <value>The common name of the field</value>
    /// <example>&quot;Access Number&quot;</example>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; }

    /// <summary>
    /// The description of the field and/or where to find it
    /// </summary>
    /// <value>The description of the field and/or where to find it</value>
    /// <example>&quot;The nine-digit access number used to login to your internet banking&quot;</example>
    [DataMember(Name = "description", EmitDefaultValue = false)]
    public string Description { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner {\n");
        sb.Append("  Type: ").Append(Type).Append('\n');
        sb.Append("  Regex: ").Append(Regex).Append('\n');
        sb.Append("  Name: ").Append(Name).Append('\n');
        sb.Append("  Description: ").Append(Description).Append('\n');
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
        return Equals(input as BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner);
    }

    /// <summary>
    /// Returns true if BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner instances are equal
    /// </summary>
    /// <param name="input">Instance of BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Type == input.Type ||
                Type.Equals(input.Type)
            ) &&
            (
                Regex == input.Regex ||
                (Regex != null &&
                 Regex.Equals(input.Regex))
            ) &&
            (
                Name == input.Name ||
                (Name != null &&
                 Name.Equals(input.Name))
            ) &&
            (
                Description == input.Description ||
                (Description != null &&
                 Description.Equals(input.Description))
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
            hashCode = (hashCode * 59) + Type.GetHashCode();
            if (Regex != null)
            {
                hashCode = (hashCode * 59) + Regex.GetHashCode();
            }

            if (Name != null)
            {
                hashCode = (hashCode * 59) + Name.GetHashCode();
            }

            if (Description != null)
            {
                hashCode = (hashCode * 59) + Description.GetHashCode();
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