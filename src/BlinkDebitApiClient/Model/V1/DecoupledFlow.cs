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
/// The details for a Decoupled flow.
/// </summary>
[DataContract(Name = "decoupled")]
[JsonConverter(typeof(JsonSubtypes), "Type")]
public class DecoupledFlow : AuthFlowDetail, IEquatable<DecoupledFlow>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name = "bank", IsRequired = true, EmitDefaultValue = true)]
    public Bank Bank { get; set; }

    /// <summary>
    /// Gets or Sets IdentifierType
    /// </summary>
    [DataMember(Name = "identifier_type", IsRequired = true, EmitDefaultValue = true)]
    public IdentifierType IdentifierType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoupledFlow" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected DecoupledFlow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoupledFlow" /> class.
    /// </summary>
    /// <param name="bank">bank (required).</param>
    /// <param name="identifierType">identifierType (required).</param>
    /// <param name="identifierValue">The identifier value. (required).</param>
    /// <param name="callbackUrl">A callback URL to call once the consent status has been updated using decoupled flow. Blink will also append the &#x60;cid&#x60; (the Consent ID) in an additional URL parameter. This is sent to your api as a GET request and will be retried up to 3 times if 5xx errors are received from your server..</param>
    /// <param name="type">Whether to use Blink Gateway, redirect or decoupled flow. (required).</param>
    public DecoupledFlow(Bank bank = default(Bank), IdentifierType identifierType = default(IdentifierType),
        string identifierValue = default(string), string callbackUrl = default(string),
        TypeEnum type = default(TypeEnum))
    {
        Bank = bank;
        IdentifierType = identifierType;
        // to ensure "identifierValue" is required (not null)
        IdentifierValue = identifierValue ??
                          throw new ArgumentNullException(
                              "identifierValue is a required property for DecoupledFlow and cannot be null");
        CallbackUrl = callbackUrl;
        Type = type;
    }

    /// <summary>
    /// The identifier value.
    /// </summary>
    /// <value>The identifier value.</value>
    [DataMember(Name = "identifier_value", IsRequired = true, EmitDefaultValue = true)]
    public string IdentifierValue { get; set; }

    /// <summary>
    /// A callback URL to call once the consent status has been updated using decoupled flow. Blink will also append the &#x60;cid&#x60; (the Consent ID) in an additional URL parameter. This is sent to your api as a GET request and will be retried up to 3 times if 5xx errors are received from your server.
    /// </summary>
    /// <value>A callback URL to call once the consent status has been updated using decoupled flow. Blink will also append the &#x60;cid&#x60; (the Consent ID) in an additional URL parameter. This is sent to your api as a GET request and will be retried up to 3 times if 5xx errors are received from your server.</value>
    /// <example>&quot;https://api.mybiller.co.nz/payments/1.0/consentresponse?secret&#x3D;SOME_SECRET&amp;id&#x3D;SOME_ID&quot;</example>
    [DataMember(Name = "callback_url", EmitDefaultValue = false)]
    public string CallbackUrl { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class DecoupledFlow {\n");
        sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append('\n');
        sb.Append("  Bank: ").Append(Bank).Append('\n');
        sb.Append("  IdentifierType: ").Append(IdentifierType).Append('\n');
        sb.Append("  IdentifierValue: ").Append(IdentifierValue).Append('\n');
        sb.Append("  CallbackUrl: ").Append(CallbackUrl).Append('\n');
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
        return Equals(input as DecoupledFlow);
    }

    /// <summary>
    /// Returns true if DecoupledFlow instances are equal
    /// </summary>
    /// <param name="input">Instance of DecoupledFlow to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(DecoupledFlow input)
    {
        if (input == null)
        {
            return false;
        }

        return base.Equals(input) &&
               (
                   Bank == input.Bank ||
                   Bank.Equals(input.Bank)
               ) && base.Equals(input) &&
               (
                   IdentifierType == input.IdentifierType ||
                   IdentifierType.Equals(input.IdentifierType)
               ) && base.Equals(input) &&
               (
                   IdentifierValue == input.IdentifierValue ||
                   (IdentifierValue != null &&
                    IdentifierValue.Equals(input.IdentifierValue))
               ) && base.Equals(input) &&
               (
                   CallbackUrl == input.CallbackUrl ||
                   (CallbackUrl != null &&
                    CallbackUrl.Equals(input.CallbackUrl))
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
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 59) + Bank.GetHashCode();
            hashCode = (hashCode * 59) + IdentifierType.GetHashCode();
            if (IdentifierValue != null)
            {
                hashCode = (hashCode * 59) + IdentifierValue.GetHashCode();
            }

            if (CallbackUrl != null)
            {
                hashCode = (hashCode * 59) + CallbackUrl.GetHashCode();
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
        return BaseValidate(validationContext);
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    protected IEnumerable<ValidationResult> BaseValidate(ValidationContext validationContext)
    {
        yield break;
    }
}