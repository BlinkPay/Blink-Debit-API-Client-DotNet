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
/// Decoupled flow hint.
/// </summary>
[DataContract(Name = "decoupled")]
public class DecoupledFlowHint : FlowHint, IEquatable<DecoupledFlowHint>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets IdentifierType
    /// </summary>
    [DataMember(Name = "identifier_type", IsRequired = true, EmitDefaultValue = true)]
    public IdentifierType IdentifierType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoupledFlowHint" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected DecoupledFlowHint()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoupledFlowHint" /> class.
    /// </summary>
    /// <param name="bank">bank (required).</param>
    /// <param name="identifierType">identifierType (required).</param>
    /// <param name="identifierValue">The identifier value. (required).</param>
    /// <param name="type">The flow hint type, i.e. Redirect or Decoupled. (required).</param>
    public DecoupledFlowHint(Bank bank = default(Bank), IdentifierType identifierType = default(IdentifierType),
        string identifierValue = default(string), TypeEnum type = TypeEnum.Decoupled)
    {
        Type = type;
        Bank = bank;
        IdentifierType = identifierType;
        // to ensure "identifierValue" is required (not null)
        IdentifierValue = identifierValue ?? throw new ArgumentNullException(
            "identifierValue is a required property for DecoupledFlowHint and cannot be null");
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
        StringBuilder sb = new StringBuilder();
        sb.Append("class DecoupledFlowHint {\n");
        sb.Append("  Type: ").Append(Type).Append('\n');
        sb.Append("  Bank: ").Append(Bank).Append('\n');
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
        return Equals(input as DecoupledFlowHint);
    }

    /// <summary>
    /// Returns true if DecoupledFlowHint instances are equal
    /// </summary>
    /// <param name="input">Instance of DecoupledFlowHint to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(DecoupledFlowHint input)
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
                Bank == input.Bank ||
                Bank.Equals(input.Bank)
            ) &&
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
            int hashCode = 41;
            hashCode = (hashCode * 59) + Type.GetHashCode();
            hashCode = (hashCode * 59) + Bank.GetHashCode();
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