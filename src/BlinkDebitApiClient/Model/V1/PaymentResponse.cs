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
/// The payment response
/// </summary>
[DataContract(Name = "Payment_Response")]
public class PaymentResponse : IEquatable<PaymentResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected PaymentResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentResponse" /> class.
    /// </summary>
    /// <param name="paymentId">The payment ID (required).</param>
    public PaymentResponse(Guid paymentId = default(Guid))
    {
        PaymentId = paymentId;
    }

    /// <summary>
    /// The payment ID
    /// </summary>
    /// <value>The payment ID</value>
    [DataMember(Name = "payment_id", IsRequired = true, EmitDefaultValue = true)]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class PaymentResponse {\n");
        sb.Append("  PaymentId: ").Append(PaymentId).Append('\n');
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
        return Equals(input as PaymentResponse);
    }

    /// <summary>
    /// Returns true if PaymentResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of PaymentResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(PaymentResponse input)
    {
        if (input == null)
        {
            return false;
        }

        return PaymentId == input.PaymentId ||
               (PaymentId != null &&
                PaymentId.Equals(input.PaymentId));
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
            if (PaymentId != null)
            {
                hashCode = (hashCode * 59) + PaymentId.GetHashCode();
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