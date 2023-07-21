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
/// The type of bank authentication flow used and details of the authentication flow.
/// </summary>
[DataContract(Name = "auth-flow")]
public class AuthFlow : IEquatable<AuthFlow>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthFlow" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected AuthFlow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthFlow" /> class.
    /// </summary>
    /// <param name="detail">detail (required).</param>
    public AuthFlow(AuthFlowDetail detail = default(AuthFlowDetail))
    {
        // to ensure "detail" is required (not null)
        Detail = detail ??
                 throw new BlinkInvalidValueException("detail is a required property for AuthFlow and cannot be null");
    }

    /// <summary>
    /// Gets or Sets Detail
    /// </summary>
    [DataMember(Name = "detail", IsRequired = true, EmitDefaultValue = true)]
    public AuthFlowDetail Detail { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class AuthFlow {\n");
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
        return Equals(input as AuthFlow);
    }

    /// <summary>
    /// Returns true if AuthFlow instances are equal
    /// </summary>
    /// <param name="input">Instance of AuthFlow to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AuthFlow input)
    {
        if (input == null)
        {
            return false;
        }

        return
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
        yield break;
    }
}