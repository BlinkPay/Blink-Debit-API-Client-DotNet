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
using JsonSubTypes;
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The details for a Gateway flow.
/// </summary>
[DataContract(Name = "gateway-flow")]
[JsonConverter(typeof(JsonSubtypes), "Type")]
public class GatewayFlow : AuthFlowDetail, IEquatable<GatewayFlow>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GatewayFlow" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected GatewayFlow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GatewayFlow" /> class.
    /// </summary>
    /// <param name="redirectUri">The URL to redirect back to once the payment is completed through the gateway. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also. (required).</param>
    /// <param name="flowHint">flowHint.</param>
    /// <param name="type">Whether to use Blink Gateway, redirect or decoupled flow. (required).</param>
    public GatewayFlow(string redirectUri = default(string),
        GatewayFlowAllOfFlowHint flowHint = default(GatewayFlowAllOfFlowHint), TypeEnum type = TypeEnum.Gateway)
    {
        // to ensure "redirectUri" is required (not null)
        RedirectUri = redirectUri ??
                      throw new BlinkInvalidValueException(
                          "redirectUri is a required property for GatewayFlow and cannot be null");
        FlowHint = flowHint;
        Type = type;
    }

    /// <summary>
    /// The URL to redirect back to once the payment is completed through the gateway. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also.
    /// </summary>
    /// <value>The URL to redirect back to once the payment is completed through the gateway. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also.</value>
    [DataMember(Name = "redirect_uri", IsRequired = true, EmitDefaultValue = true)]
    public string RedirectUri { get; set; }

    /// <summary>
    /// Gets or Sets FlowHint
    /// </summary>
    [DataMember(Name = "flow_hint", EmitDefaultValue = false)]
    public GatewayFlowAllOfFlowHint FlowHint { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GatewayFlow {\n");
        sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append('\n');
        sb.Append("  RedirectUri: ").Append(RedirectUri).Append('\n');
        sb.Append("  FlowHint: ").Append(FlowHint).Append('\n');
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
        return Equals(input as GatewayFlow);
    }

    /// <summary>
    /// Returns true if GatewayFlow instances are equal
    /// </summary>
    /// <param name="input">Instance of GatewayFlow to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GatewayFlow input)
    {
        if (input == null)
        {
            return false;
        }

        return base.Equals(input) &&
               (
                   RedirectUri == input.RedirectUri ||
                   (RedirectUri != null &&
                    RedirectUri.Equals(input.RedirectUri))
               ) && base.Equals(input) &&
               (
                   FlowHint == input.FlowHint ||
                   (FlowHint != null &&
                    FlowHint.Equals(input.FlowHint))
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
            if (RedirectUri != null)
            {
                hashCode = (hashCode * 59) + RedirectUri.GetHashCode();
            }

            if (FlowHint != null)
            {
                hashCode = (hashCode * 59) + FlowHint.GetHashCode();
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