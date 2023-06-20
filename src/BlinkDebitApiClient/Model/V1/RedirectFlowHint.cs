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
/// Redirect flow hint.
/// </summary>
[DataContract(Name = "redirect-flow-hint")]
public class RedirectFlowHint : FlowHint, IEquatable<RedirectFlowHint>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectFlowHint" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected RedirectFlowHint()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectFlowHint" /> class.
    /// </summary>
    /// <param name="type">The flow hint type, i.e. Redirect or Decoupled. (required).</param>
    /// <param name="bank">bank (required).</param>
    public RedirectFlowHint(TypeEnum type = default(TypeEnum), Bank bank = default(Bank))
    {
        Type = type;
        Bank = bank;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class RedirectFlowHint {\n");
        sb.Append("  Type: ").Append(Type).Append('\n');
        sb.Append("  Bank: ").Append(Bank).Append('\n');
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
        return Equals(input as RedirectFlowHint);
    }

    /// <summary>
    /// Returns true if RedirectFlowHint instances are equal
    /// </summary>
    /// <param name="input">Instance of RedirectFlowHint to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RedirectFlowHint input)
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
            hashCode = (hashCode * 59) + Bank.GetHashCode();
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