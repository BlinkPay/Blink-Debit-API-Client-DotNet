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

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// PCR (Particulars, code, reference) details.
/// </summary>
[DataContract(Name = "pcr")]
public class Pcr : IEquatable<Pcr>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pcr" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected Pcr()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pcr" /> class.
    /// </summary>
    /// <param name="particulars">The particulars (required).</param>
    /// <param name="code">The code.</param>
    /// <param name="reference">The reference.</param>
    public Pcr(string particulars = default(string), string code = default(string),
        string reference = default(string))
    {
        // to ensure "particulars" is required (not null)
        Particulars = particulars ?? throw new ArgumentNullException("particulars is a required property for Pcr and cannot be null");
        Code = code;
        Reference = reference;
    }

    /// <summary>
    /// The particulars
    /// </summary>
    /// <value>The particulars</value>
    [DataMember(Name = "particulars", IsRequired = true, EmitDefaultValue = true)]
    public string Particulars { get; set; }

    /// <summary>
    /// The code
    /// </summary>
    /// <value>The code</value>
    [DataMember(Name = "code", EmitDefaultValue = false)]
    public string Code { get; set; }

    /// <summary>
    /// The reference
    /// </summary>
    /// <value>The reference</value>
    [DataMember(Name = "reference", EmitDefaultValue = false)]
    public string Reference { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Pcr {\n");
        sb.Append("  Particulars: ").Append(Particulars).Append('\n');
        sb.Append("  Code: ").Append(Code).Append('\n');
        sb.Append("  Reference: ").Append(Reference).Append('\n');
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
        return Equals(input as Pcr);
    }

    /// <summary>
    /// Returns true if Pcr instances are equal
    /// </summary>
    /// <param name="input">Instance of Pcr to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Pcr input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Particulars == input.Particulars ||
                (Particulars != null &&
                 Particulars.Equals(input.Particulars))
            ) &&
            (
                Code == input.Code ||
                (Code != null &&
                 Code.Equals(input.Code))
            ) &&
            (
                Reference == input.Reference ||
                (Reference != null &&
                 Reference.Equals(input.Reference))
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
            if (Particulars != null)
            {
                hashCode = (hashCode * 59) + Particulars.GetHashCode();
            }

            if (Code != null)
            {
                hashCode = (hashCode * 59) + Code.GetHashCode();
            }

            if (Reference != null)
            {
                hashCode = (hashCode * 59) + Reference.GetHashCode();
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
        // Particulars (string) maxLength
        if (Particulars != null && Particulars.Length > 12)
        {
            yield return new ValidationResult(
                "Invalid value for Particulars, length must be less than 12.", new[] { "Particulars" });
        }

        // Particulars (string) minLength
        if (Particulars != null && Particulars.Length < 1)
        {
            yield return new ValidationResult(
                "Invalid value for Particulars, length must be greater than 1.", new[] { "Particulars" });
        }

        // Particulars (string) pattern
        var regexParticulars = new Regex(@"[a-zA-Z0-9- &#\?:_\/,\.']{1,12}", RegexOptions.CultureInvariant);
        if (false == regexParticulars.Match(Particulars).Success)
        {
            yield return new ValidationResult(
                "Invalid value for Particulars, must match a pattern of " + regexParticulars,
                new[] { "Particulars" });
        }

        // Code (string) maxLength
        if (Code != null && Code.Length > 12)
        {
            yield return new ValidationResult(
                "Invalid value for Code, length must be less than 12.", new[] { "Code" });
        }

        // Code (string) pattern
        var regexCode = new Regex(@"[a-zA-Z0-9- &#\?:_\/,\.']{0,12}", RegexOptions.CultureInvariant);
        if (false == regexCode.Match(Code).Success)
        {
            yield return new ValidationResult(
                "Invalid value for Code, must match a pattern of " + regexCode, new[] { "Code" });
        }

        // Reference (string) maxLength
        if (Reference != null && Reference.Length > 12)
        {
            yield return new ValidationResult(
                "Invalid value for Reference, length must be less than 12.", new[] { "Reference" });
        }

        // Reference (string) pattern
        var regexReference = new Regex(@"[a-zA-Z0-9- &#\?:_\/,\.']{0,12}", RegexOptions.CultureInvariant);
        if (false == regexReference.Match(Reference).Success)
        {
            yield return new ValidationResult(
                "Invalid value for Reference, must match a pattern of " + regexReference, new[] { "Reference" });
        }
    }
}