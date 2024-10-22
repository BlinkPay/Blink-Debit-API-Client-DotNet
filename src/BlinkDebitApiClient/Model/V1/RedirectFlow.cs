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
/// The details for a Redirect flow.
/// </summary>
[DataContract(Name = "redirect")]
[JsonConverter(typeof(JsonSubtypes), "Type")]
public class RedirectFlow : AuthFlowDetail, IEquatable<RedirectFlow>, IValidatableObject
{
    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name = "bank", EmitDefaultValue = true)]
    public Bank Bank { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectFlow" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected RedirectFlow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectFlow" /> class.
    /// </summary>
    /// <param name="redirectUri">The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also. (required).</param>
    /// <param name="bank">bank (required).</param>
    /// <param name="redirectToApp">Whether the redirect URI goes back to an app directly. If this value is true, the app will receive code and state parameters with this redirection. The app must pass these through to us at: https://debit.blinkpay.co.nz/bank/1.0/return?state&#x3D;{state}&amp;code&#x3D;{code}, along with other query parameters like error. Applies only to Redirect flow. (default to false).</param>
    /// <param name="type">Whether to use Blink Gateway, redirect or decoupled flow. (required).</param>
    public RedirectFlow(string redirectUri = default(string), Bank bank = default(Bank),
        bool? redirectToApp = false, TypeEnum type = TypeEnum.Redirect) : base()
    {
        // to ensure "redirectUri" is required (not null)
        RedirectUri = redirectUri ??
                      throw new BlinkInvalidValueException(
                          "redirectUri is a required property for RedirectFlow and cannot be null");
        Bank = bank;
        Type = type;
        RedirectToApp = redirectToApp;
    }

    /// <summary>
    /// The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also.
    /// </summary>
    /// <value>The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The &#x60;cid&#x60; (Consent ID) will be added as a URL parameter. If there is an error, an &#x60;error&#x60; parameter will be appended also.</value>
    [DataMember(Name = "redirect_uri", IsRequired = true, EmitDefaultValue = true)]
    public string RedirectUri { get; set; }

    /// <summary>
    /// Whether the redirect URI goes back to an app directly. If this value is true, the app will receive code and state parameters with this redirection. The app must pass these through to us at: https://debit.blinkpay.co.nz/bank/1.0/return?state&#x3D;{state}&amp;code&#x3D;{code}, along with other query parameters like error. Applies only to Redirect flow.
    /// </summary>
    /// <value>Whether the redirect URI goes back to an app directly. If this value is true, the app will receive code and state parameters with this redirection. The app must pass these through to us at: https://debit.blinkpay.co.nz/bank/1.0/return?state&#x3D;{state}&amp;code&#x3D;{code}, along with other query parameters like error. Applies only to Redirect flow.</value>
    [DataMember(Name="redirect_to_app", EmitDefaultValue=false)]
    public bool? RedirectToApp { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class RedirectFlow {\n");
        sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append('\n');
        sb.Append("  RedirectUri: ").Append(RedirectUri).Append('\n');
        sb.Append("  Bank: ").Append(Bank).Append('\n');
        sb.Append("  RedirectToApp: ").Append(RedirectToApp).Append("\n");
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
        return Equals(input as RedirectFlow);
    }

    /// <summary>
    /// Returns true if RedirectFlow instances are equal
    /// </summary>
    /// <param name="input">Instance of RedirectFlow to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RedirectFlow input)
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
                   Bank == input.Bank ||
                   Bank.Equals(input.Bank)
               )&& base.Equals(input) && 
               (
                   RedirectToApp == input.RedirectToApp ||
                   (RedirectToApp != null &&
                    RedirectToApp.Equals(input.RedirectToApp))
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
            if (Bank != null)
            {
                hashCode = (hashCode * 59) + Bank.GetHashCode();
            }
            if (RedirectToApp != null)
            {
                hashCode = (hashCode * 59) + RedirectToApp.GetHashCode();
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