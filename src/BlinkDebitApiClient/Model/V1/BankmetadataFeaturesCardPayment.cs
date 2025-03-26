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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The card payment feature
/// </summary>
[DataContract]
public partial class BankmetadataFeaturesCardPayment : IEquatable<BankmetadataFeaturesCardPayment>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankmetadataFeaturesCardPayment" /> class.
    /// </summary>
    /// <param name="enabled">Whether card payment is enabled. (required).</param>
    /// <param name="allowedCardPaymentTypes">The allowed card payment types. (required).</param>
    /// <param name="allowedCardNetworks">The allowed card networks. (required).</param>
    public BankmetadataFeaturesCardPayment(bool? enabled = default(bool?),
        List<CardPaymentType> allowedCardPaymentTypes = default(List<CardPaymentType>),
        List<CardNetwork> allowedCardNetworks = default(List<CardNetwork>))
    {
        // to ensure "enabled" is required (not null)
        if (enabled == null)
        {
            throw new InvalidDataException(
                "enabled is a required property for BankmetadataFeaturesCardPayment and cannot be null");
        }
        else
        {
            this.Enabled = enabled;
        }

        // to ensure "allowedCardPaymentTypes" is required (not null)
        if (allowedCardPaymentTypes == null)
        {
            throw new InvalidDataException(
                "allowedCardPaymentTypes is a required property for BankmetadataFeaturesCardPayment and cannot be null");
        }
        else
        {
            this.AllowedCardPaymentTypes = allowedCardPaymentTypes;
        }

        // to ensure "allowedCardNetworks" is required (not null)
        if (allowedCardNetworks == null)
        {
            throw new InvalidDataException(
                "allowedCardNetworks is a required property for BankmetadataFeaturesCardPayment and cannot be null");
        }
        else
        {
            this.AllowedCardNetworks = allowedCardNetworks;
        }
    }

    /// <summary>
    /// Whether card payment is enabled.
    /// </summary>
    /// <value>Whether card payment is enabled.</value>
    [DataMember(Name = "enabled", EmitDefaultValue = false)]
    public bool? Enabled { get; set; }

    /// <summary>
    /// The allowed card payment types.
    /// </summary>
    /// <value>The allowed card payment types.</value>
    [DataMember(Name = "allowed_card_payment_types", EmitDefaultValue = false)]
    public List<CardPaymentType> AllowedCardPaymentTypes { get; set; }

    /// <summary>
    /// The allowed card networks.
    /// </summary>
    /// <value>The allowed card networks.</value>
    [DataMember(Name = "allowed_card_networks", EmitDefaultValue = false)]
    public List<CardNetwork> AllowedCardNetworks { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class BankmetadataFeaturesCardPayment {\n");
        sb.Append("  Enabled: ").Append(Enabled).Append("\n");
        sb.Append("  AllowedCardPaymentTypes: ").Append(AllowedCardPaymentTypes).Append("\n");
        sb.Append("  AllowedCardNetworks: ").Append(AllowedCardNetworks).Append("\n");
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
        return this.Equals(input as BankmetadataFeaturesCardPayment);
    }

    /// <summary>
    /// Returns true if BankmetadataFeaturesCardPayment instances are equal
    /// </summary>
    /// <param name="input">Instance of BankmetadataFeaturesCardPayment to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BankmetadataFeaturesCardPayment input)
    {
        if (input == null)
            return false;

        return
            (
                this.Enabled == input.Enabled ||
                (this.Enabled != null &&
                 this.Enabled.Equals(input.Enabled))
            ) &&
            (
                this.AllowedCardPaymentTypes == input.AllowedCardPaymentTypes ||
                this.AllowedCardPaymentTypes != null &&
                input.AllowedCardPaymentTypes != null &&
                this.AllowedCardPaymentTypes.SequenceEqual(input.AllowedCardPaymentTypes)
            ) &&
            (
                this.AllowedCardNetworks == input.AllowedCardNetworks ||
                this.AllowedCardNetworks != null &&
                input.AllowedCardNetworks != null &&
                this.AllowedCardNetworks.SequenceEqual(input.AllowedCardNetworks)
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
            int hashCode = 41;
            if (this.Enabled != null)
                hashCode = hashCode * 59 + this.Enabled.GetHashCode();
            if (this.AllowedCardPaymentTypes != null)
                hashCode = hashCode * 59 + this.AllowedCardPaymentTypes.GetHashCode();
            if (this.AllowedCardNetworks != null)
                hashCode = hashCode * 59 + this.AllowedCardNetworks.GetHashCode();
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