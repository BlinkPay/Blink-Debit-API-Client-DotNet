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
/// The quick payment request
/// </summary>
[DataContract(Name = "Create_Quick_Payment_Response")]
public class CreateQuickPaymentResponse : IEquatable<CreateQuickPaymentResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateQuickPaymentResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected CreateQuickPaymentResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateQuickPaymentResponse" /> class.
    /// </summary>
    /// <param name="quickPaymentId">The quick payment ID (required).</param>
    /// <param name="redirectUri">The URL to redirect the user to, returned if the flow type is \&quot;redirect\&quot; or \&quot;gateway\&quot;.</param>
    public CreateQuickPaymentResponse(Guid quickPaymentId = default(Guid), string redirectUri = default(string))
    {
        QuickPaymentId = quickPaymentId;
        RedirectUri = redirectUri;
    }

    /// <summary>
    /// The quick payment ID
    /// </summary>
    /// <value>The quick payment ID</value>
    [DataMember(Name = "quick_payment_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid QuickPaymentId { get; set; }

    /// <summary>
    /// The URL to redirect the user to, returned if the flow type is \&quot;redirect\&quot; or \&quot;gateway\&quot;
    /// </summary>
    /// <value>The URL to redirect the user to, returned if the flow type is \&quot;redirect\&quot; or \&quot;gateway\&quot;</value>
    [DataMember(Name = "redirect_uri", EmitDefaultValue = false)]
    public string RedirectUri { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class CreateQuickPaymentResponse {\n");
        sb.Append("  QuickPaymentId: ").Append(QuickPaymentId).Append('\n');
        sb.Append("  RedirectUri: ").Append(RedirectUri).Append('\n');
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
        return Equals(input as CreateQuickPaymentResponse);
    }

    /// <summary>
    /// Returns true if CreateQuickPaymentResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of CreateQuickPaymentResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(CreateQuickPaymentResponse input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                QuickPaymentId == input.QuickPaymentId ||
                (QuickPaymentId != null &&
                 QuickPaymentId.Equals(input.QuickPaymentId))
            ) &&
            (
                RedirectUri == input.RedirectUri ||
                (RedirectUri != null &&
                 RedirectUri.Equals(input.RedirectUri))
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
            if (QuickPaymentId != null)
            {
                hashCode = (hashCode * 59) + QuickPaymentId.GetHashCode();
            }

            if (RedirectUri != null)
            {
                hashCode = (hashCode * 59) + RedirectUri.GetHashCode();
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