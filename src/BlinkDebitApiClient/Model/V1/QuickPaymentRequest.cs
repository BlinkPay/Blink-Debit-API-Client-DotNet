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
using JsonSubTypes;
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The model for a quick payment, relating to a single consent and a one-off payment.
/// </summary>
[DataContract(Name = "quick")]
[JsonConverter(typeof(JsonSubtypes), "Type")]
public class QuickPaymentRequest : SingleConsentRequest, IEquatable<QuickPaymentRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuickPaymentRequest" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected QuickPaymentRequest()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuickPaymentRequest" /> class.
    /// </summary>
    /// <param name="type">Whether the consent is single or enduring. (required).</param>
    /// <param name="flow">flow (required).</param>
    /// <param name="pcr">pcr (required).</param>
    /// <param name="amount">amount (required).</param>
    public QuickPaymentRequest(TypeEnum type = default(TypeEnum), AuthFlow flow = default(AuthFlow),
        Pcr pcr = default(Pcr), Amount amount = default(Amount)): base(flow, pcr, amount, TypeEnum.Single)
    {
        // Type = type;
        // Flow = flow;
        // Pcr = pcr;
        // Amount = amount;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class QuickPaymentRequest {\n");
        sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public override string ToJson()
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
        return Equals(input as QuickPaymentRequest);
    }

    /// <summary>
    /// Returns true if QuickPaymentRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of QuickPaymentRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(QuickPaymentRequest input)
    {
        if (input == null)
        {
            return false;
        }

        return base.Equals(input);
    }

    /// <summary>
    /// Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        int hashCode = base.GetHashCode();
        return hashCode;
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        return BaseValidate(validationContext);
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    protected IEnumerable<ValidationResult> BaseValidate(ValidationContext validationContext)
    {
        foreach (var x in base.BaseValidate(validationContext))
        {
            yield return x;
        }
    }
}