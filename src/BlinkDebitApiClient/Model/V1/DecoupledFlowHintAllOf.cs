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
/// The decoupled flow hint identifier
/// </summary>
[DataContract(Name = "decoupled_flow_hint_allOf")]
public class DecoupledFlowHintAllOf : IEquatable<DecoupledFlowHintAllOf>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets IdentifierType
    /// </summary>
    [DataMember(Name = "identifier_type", IsRequired = true, EmitDefaultValue = true)]
    public IdentifierType IdentifierType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoupledFlowHintAllOf" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected DecoupledFlowHintAllOf()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoupledFlowHintAllOf" /> class.
    /// </summary>
    /// <param name="identifierType">identifierType (required).</param>
    /// <param name="identifierValue">The identifier value. (required).</param>
    public DecoupledFlowHintAllOf(IdentifierType identifierType = default(IdentifierType),
        string identifierValue = default(string))
    {
        IdentifierType = identifierType;
        // to ensure "identifierValue" is required (not null)
        IdentifierValue = identifierValue ??
                          throw new BlinkInvalidValueException(
                              "identifierValue is a required property for DecoupledFlowHintAllOf and cannot be null");
    }

    /// <summary>
    /// The identifier value.
    /// </summary>
    /// <value>The identifier value.</value>
    [DataMember(Name = "identifier_value", IsRequired = true, EmitDefaultValue = true)]
    public string IdentifierValue { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class DecoupledFlowHintAllOf {\n");
        sb.Append("  IdentifierType: ").Append(IdentifierType).Append('\n');
        sb.Append("  IdentifierValue: ").Append(IdentifierValue).Append('\n');
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
        return Equals(input as DecoupledFlowHintAllOf);
    }

    /// <summary>
    /// Returns true if DecoupledFlowHintAllOf instances are equal
    /// </summary>
    /// <param name="input">Instance of DecoupledFlowHintAllOf to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(DecoupledFlowHintAllOf input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                IdentifierType == input.IdentifierType ||
                IdentifierType.Equals(input.IdentifierType)
            ) &&
            (
                IdentifierValue == input.IdentifierValue ||
                (IdentifierValue != null &&
                 IdentifierValue.Equals(input.IdentifierValue))
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
            hashCode = (hashCode * 59) + IdentifierType.GetHashCode();
            if (IdentifierValue != null)
            {
                hashCode = (hashCode * 59) + IdentifierValue.GetHashCode();
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