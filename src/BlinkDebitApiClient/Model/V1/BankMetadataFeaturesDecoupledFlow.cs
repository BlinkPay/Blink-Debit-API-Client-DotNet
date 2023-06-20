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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The decoupled flow bank feature
/// </summary>
[DataContract(Name = "bank_metadata_features_decoupled_flow")]
public class BankMetadataFeaturesDecoupledFlow : IEquatable<BankMetadataFeaturesDecoupledFlow>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeaturesDecoupledFlow" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected BankMetadataFeaturesDecoupledFlow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankMetadataFeaturesDecoupledFlow" /> class.
    /// </summary>
    /// <param name="enabled">Whether the Decoupled Flow is enabled. (required).</param>
    /// <param name="availableIdentifiers">If enabled, will show the available fields to use to identify the customer with their bank..</param>
    /// <param name="requestTimeout">ISO8601 time duration until the decoupled flow consent request times out (required).</param>
    public BankMetadataFeaturesDecoupledFlow(bool enabled = default(bool),
        List<BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner> availableIdentifiers =
            default(List<BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner>),
        string requestTimeout = default(string))
    {
        Enabled = enabled;
        // to ensure "requestTimeout" is required (not null)
        RequestTimeout = requestTimeout ?? throw new ArgumentNullException(
            "requestTimeout is a required property for BankMetadataFeaturesDecoupledFlow and cannot be null");
        AvailableIdentifiers = availableIdentifiers;
    }

    /// <summary>
    /// Whether the Decoupled Flow is enabled.
    /// </summary>
    /// <value>Whether the Decoupled Flow is enabled.</value>
    [DataMember(Name = "enabled", IsRequired = false, EmitDefaultValue = true)]
    public bool Enabled { get; set; }

    /// <summary>
    /// If enabled, will show the available fields to use to identify the customer with their bank.
    /// </summary>
    /// <value>If enabled, will show the available fields to use to identify the customer with their bank.</value>
    [DataMember(Name = "available_identifiers", EmitDefaultValue = false)]
    public List<BankMetadataFeaturesDecoupledFlowAvailableIdentifiersInner> AvailableIdentifiers { get; set; }

    /// <summary>
    /// ISO8601 time duration until the decoupled flow consent request times out
    /// </summary>
    /// <value>ISO8601 time duration until the decoupled flow consent request times out</value>
    /// <example>&quot;P1DT00H00M00S&quot;</example>
    [DataMember(Name = "request_timeout", IsRequired = false, EmitDefaultValue = true)]
    public string RequestTimeout { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class BankMetadataFeaturesDecoupledFlow {\n");
        sb.Append("  Enabled: ").Append(Enabled).Append('\n');
        sb.Append("  AvailableIdentifiers: ").Append(AvailableIdentifiers).Append('\n');
        sb.Append("  RequestTimeout: ").Append(RequestTimeout).Append('\n');
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
        return Equals(input as BankMetadataFeaturesDecoupledFlow);
    }

    /// <summary>
    /// Returns true if BankMetadataFeaturesDecoupledFlow instances are equal
    /// </summary>
    /// <param name="input">Instance of BankMetadataFeaturesDecoupledFlow to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BankMetadataFeaturesDecoupledFlow input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Enabled == input.Enabled ||
                Enabled.Equals(input.Enabled)
            ) &&
            (
                AvailableIdentifiers == input.AvailableIdentifiers ||
                AvailableIdentifiers != null &&
                input.AvailableIdentifiers != null &&
                AvailableIdentifiers.SequenceEqual(input.AvailableIdentifiers)
            ) &&
            (
                RequestTimeout == input.RequestTimeout ||
                (RequestTimeout != null &&
                 RequestTimeout.Equals(input.RequestTimeout))
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
            hashCode = (hashCode * 59) + Enabled.GetHashCode();
            if (AvailableIdentifiers != null)
            {
                hashCode = (hashCode * 59) + AvailableIdentifiers.GetHashCode();
            }

            if (RequestTimeout != null)
            {
                hashCode = (hashCode * 59) + RequestTimeout.GetHashCode();
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