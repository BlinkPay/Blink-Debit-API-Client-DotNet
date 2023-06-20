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
/// Redirect consent flow
/// </summary>
[DataContract(Name = "redirect_flow_allOf")]
public class RedirectFlowAllOf : IEquatable<RedirectFlowAllOf>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name = "bank", IsRequired = true, EmitDefaultValue = true)]
    public Bank Bank { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectFlowAllOf" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected RedirectFlowAllOf()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectFlowAllOf" /> class.
    /// </summary>
    /// <param name="redirectUri">The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also. (required).</param>
    /// <param name="bank">bank (required).</param>
    public RedirectFlowAllOf(string redirectUri = default(string), Bank bank = default(Bank))
    {
        // to ensure "redirectUri" is required (not null)
        RedirectUri = redirectUri ?? throw new ArgumentNullException(
            "redirectUri is a required property for RedirectFlowAllOf and cannot be null");
        Bank = bank;
    }

    /// <summary>
    /// The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also.
    /// </summary>
    /// <value>The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also.</value>
    [DataMember(Name = "redirect_uri", IsRequired = true, EmitDefaultValue = true)]
    public string RedirectUri { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class RedirectFlowAllOf {\n");
        sb.Append("  RedirectUri: ").Append(RedirectUri).Append('\n');
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
        return Equals(input as RedirectFlowAllOf);
    }

    /// <summary>
    /// Returns true if RedirectFlowAllOf instances are equal
    /// </summary>
    /// <param name="input">Instance of RedirectFlowAllOf to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RedirectFlowAllOf input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                RedirectUri == input.RedirectUri ||
                (RedirectUri != null &&
                 RedirectUri.Equals(input.RedirectUri))
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
            if (RedirectUri != null)
            {
                hashCode = (hashCode * 59) + RedirectUri.GetHashCode();
            }

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