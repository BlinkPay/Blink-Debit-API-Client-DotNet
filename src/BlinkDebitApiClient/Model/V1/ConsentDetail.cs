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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The consent details
/// </summary>
[JsonConverter(typeof(ConsentDetailJsonConverter))]
[DataContract(Name = "consent_detail")]
public class ConsentDetail : AbstractOpenAPISchema, IEquatable<ConsentDetail>, IValidatableObject
{
    /// <summary>
    /// The type of consent (single or enduring).
    /// </summary>
    /// <value>The type of consent (single or enduring).</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum
    {
        /// <summary>
        /// Enum Single for value: single
        /// </summary>
        [EnumMember(Value = "single")] Single,

        /// <summary>
        /// Enum Enduring for value: enduring
        /// </summary>
        [EnumMember(Value = "enduring")] Enduring
    }


    /// <summary>
    /// The type of payment (single or enduring).
    /// </summary>
    /// <value>The type of payment (single or enduring).</value>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsentDetail" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public ConsentDetail()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsentDetail" /> class
    /// with the <see cref="EnduringConsentRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of EnduringConsentRequest.</param>
    public ConsentDetail(EnduringConsentRequest actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsentDetail" /> class
    /// with the <see cref="SingleConsentRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SingleConsentRequest.</param>
    public ConsentDetail(SingleConsentRequest actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
    }


    private Object _actualInstance;

    /// <summary>
    /// Gets or Sets ActualInstance
    /// </summary>
    public override Object ActualInstance
    {
        get => _actualInstance;
        set
        {
            if (value.GetType() == typeof(EnduringConsentRequest))
            {
                _actualInstance = value;
            }
            else if (value.GetType() == typeof(SingleConsentRequest))
            {
                _actualInstance = value;
            }
            else
            {
                throw new ArgumentException(
                    "Invalid instance found. Must be the following types: EnduringConsentRequest, SingleConsentRequest");
            }
        }
    }

    /// <summary>
    /// Get the actual instance of `EnduringConsentRequest`. If the actual instance is not `EnduringConsentRequest`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of EnduringConsentRequest</returns>
    public EnduringConsentRequest GetEnduringConsentRequest()
    {
        return (EnduringConsentRequest)ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `SingleConsentRequest`. If the actual instance is not `SingleConsentRequest`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of SingleConsentRequest</returns>
    public SingleConsentRequest GetSingleConsentRequest()
    {
        return (SingleConsentRequest)ActualInstance;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class ConsentDetail {\n");
        sb.Append("  ActualInstance: ").Append(ActualInstance).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public override string ToJson()
    {
        return JsonConvert.SerializeObject(ActualInstance, SerializerSettings);
    }

    /// <summary>
    /// Converts the JSON string into an instance of ConsentDetail
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of ConsentDetail</returns>
    public static ConsentDetail FromJson(string jsonString)
    {
        ConsentDetail newConsentDetail = null;

        if (string.IsNullOrEmpty(jsonString))
        {
            return newConsentDetail;
        }

        try
        {
            var discriminatorObj = JObject.Parse(jsonString)["type"];
            var discriminatorValue = discriminatorObj == null ? string.Empty : discriminatorObj.ToString();
            switch (discriminatorValue)
            {
                case "enduring":
                    newConsentDetail =
                        new ConsentDetail(JsonConvert.DeserializeObject<EnduringConsentRequest>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    newConsentDetail.Type = TypeEnum.Enduring;
                    return newConsentDetail;
                case "single":
                    newConsentDetail = new ConsentDetail(JsonConvert.DeserializeObject<SingleConsentRequest>(jsonString,
                        AdditionalPropertiesSerializerSettings));
                    newConsentDetail.Type = TypeEnum.Single;
                    return newConsentDetail;
                case "quick":
                    newConsentDetail = new ConsentDetail(JsonConvert.DeserializeObject<QuickPaymentRequest>(jsonString,
                        AdditionalPropertiesSerializerSettings));
                    newConsentDetail.Type = TypeEnum.Single;
                    return newConsentDetail;
                default:
                    Debug.WriteLine(
                        $"Failed to lookup discriminator value `{discriminatorValue}` for ConsentDetail. Possible values: enduring-consent-request single-consent-request");
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to parse the json data : `{0}` {1}", jsonString, ex);
        }

        var match = 0;
        var matchedTypes = new List<string>();

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(EnduringConsentRequest).GetProperty("AdditionalProperties") == null)
            {
                newConsentDetail =
                    new ConsentDetail(
                        JsonConvert.DeserializeObject<EnduringConsentRequest>(jsonString,
                            SerializerSettings));
            }
            else
            {
                newConsentDetail = new ConsentDetail(JsonConvert.DeserializeObject<EnduringConsentRequest>(jsonString,
                    AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("EnduringConsentRequest");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into EnduringConsentRequest: {1}", jsonString, exception);
        }

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(SingleConsentRequest).GetProperty("AdditionalProperties") == null)
            {
                newConsentDetail =
                    new ConsentDetail(
                        JsonConvert.DeserializeObject<SingleConsentRequest>(jsonString,
                            SerializerSettings));
            }
            else
            {
                newConsentDetail = new ConsentDetail(JsonConvert.DeserializeObject<SingleConsentRequest>(jsonString,
                    AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("SingleConsentRequest");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into SingleConsentRequest: {1}", jsonString, exception);
        }

        if (match == 0)
        {
            throw new InvalidDataException("The JSON string `" + jsonString +
                                           "` cannot be deserialized into any schema defined.");
        }

        if (match > 1)
        {
            throw new InvalidDataException("The JSON string `" + jsonString +
                                           "` incorrectly matches more than one schema (should be exactly one match): " +
                                           matchedTypes);
        }

        // deserialization is considered successful at this point if no exception has been thrown.
        return newConsentDetail;
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as ConsentDetail);
    }

    /// <summary>
    /// Returns true if ConsentDetail instances are equal
    /// </summary>
    /// <param name="input">Instance of ConsentDetail to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ConsentDetail input)
    {
        if (input == null)
            return false;

        return ActualInstance.Equals(input.ActualInstance);
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
            if (ActualInstance != null)
                hashCode = hashCode * 59 + ActualInstance.GetHashCode();
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

/// <summary>
/// Custom JSON converter for ConsentDetail
/// </summary>
public class ConsentDetailJsonConverter : JsonConverter
{
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue((string)(typeof(ConsentDetail).GetMethod("ToJson").Invoke(value, null)));
    }

    /// <summary>
    /// To convert a JSON string into an object
    /// </summary>
    /// <param name="reader">JSON reader</param>
    /// <param name="objectType">Object type</param>
    /// <param name="existingValue">Existing value</param>
    /// <param name="serializer">JSON Serializer</param>
    /// <returns>The object converted from the JSON string</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.Null)
        {
            return ConsentDetail.FromJson(JObject.Load(reader).ToString(Formatting.None));
        }

        return null;
    }

    /// <summary>
    /// Check if the object can be converted
    /// </summary>
    /// <param name="objectType">Object type</param>
    /// <returns>True if the object can be converted</returns>
    public override bool CanConvert(Type objectType)
    {
        return false;
    }
}