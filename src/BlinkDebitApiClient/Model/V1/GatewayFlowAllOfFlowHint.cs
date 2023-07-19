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
using BlinkDebitApiClient.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The gateway flow hint
/// </summary>
[JsonConverter(typeof(GatewayFlowAllOfFlowHintJsonConverter))]
[DataContract(Name = "gateway_flow_allOf_flow_hint")]
public class GatewayFlowAllOfFlowHint : AbstractOpenAPISchema, IEquatable<GatewayFlowAllOfFlowHint>, IValidatableObject
{
    /// <summary>
    /// The flow hint type, i.e. Redirect or Decoupled.
    /// </summary>
    /// <value>The flow hint type, i.e. Redirect or Decoupled.</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum
    {
        /// <summary>
        /// Enum Redirect for value: redirect
        /// </summary>
        [EnumMember(Value = "redirect")] Redirect,

        /// <summary>
        /// Enum Decoupled for value: decoupled
        /// </summary>
        [EnumMember(Value = "decoupled")] Decoupled
    }


    /// <summary>
    /// The flow hint type, i.e. Redirect or Decoupled.
    /// </summary>
    /// <value>The flow hint type, i.e. Redirect or Decoupled.</value>
    /// <example>&quot;redirect&quot;</example>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GatewayFlowAllOfFlowHint" /> class
    /// with the <see cref="RedirectFlowHint" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of RedirectFlowHint.</param>
    public GatewayFlowAllOfFlowHint(RedirectFlowHint actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new BlinkInvalidValueException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GatewayFlowAllOfFlowHint" /> class
    /// with the <see cref="DecoupledFlowHint" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of DecoupledFlowHint.</param>
    public GatewayFlowAllOfFlowHint(DecoupledFlowHint actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new BlinkInvalidValueException("Invalid instance found. Must not be null.");
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
            if (value.GetType() == typeof(DecoupledFlowHint))
            {
                _actualInstance = value;
            }
            else if (value.GetType() == typeof(RedirectFlowHint))
            {
                _actualInstance = value;
            }
            else
            {
                throw new BlinkInvalidValueException(
                    "Invalid instance found. Must be the following types: DecoupledFlowHint, RedirectFlowHint");
            }
        }
    }

    /// <summary>
    /// Get the actual instance of `RedirectFlowHint`. If the actual instance is not `RedirectFlowHint`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of RedirectFlowHint</returns>
    public RedirectFlowHint GetRedirectFlowHint()
    {
        return (RedirectFlowHint)ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `DecoupledFlowHint`. If the actual instance is not `DecoupledFlowHint`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of DecoupledFlowHint</returns>
    public DecoupledFlowHint GetDecoupledFlowHint()
    {
        return (DecoupledFlowHint)ActualInstance;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class GatewayFlowAllOfFlowHint {\n");
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
    /// Converts the JSON string into an instance of GatewayFlowAllOfFlowHint
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of GatewayFlowAllOfFlowHint</returns>
    public static GatewayFlowAllOfFlowHint FromJson(string jsonString)
    {
        GatewayFlowAllOfFlowHint newGatewayFlowAllOfFlowHint = null;

        if (string.IsNullOrEmpty(jsonString))
        {
            return newGatewayFlowAllOfFlowHint;
        }

        try
        {
            var discriminatorObj = JObject.Parse(jsonString)["type"];
            string discriminatorValue = discriminatorObj == null ? string.Empty : discriminatorObj.ToString();
            switch (discriminatorValue)
            {
                case "decoupled":
                    newGatewayFlowAllOfFlowHint = new GatewayFlowAllOfFlowHint(
                        JsonConvert.DeserializeObject<DecoupledFlowHint>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newGatewayFlowAllOfFlowHint;
                case "redirect":
                    newGatewayFlowAllOfFlowHint = new GatewayFlowAllOfFlowHint(
                        JsonConvert.DeserializeObject<RedirectFlowHint>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newGatewayFlowAllOfFlowHint;
                default:
                    Debug.WriteLine(string.Format(
                        "Failed to lookup discriminator value `{0}` for GatewayFlowAllOfFlowHint. Possible values: decoupled-flow-hint redirect-flow-hint",
                        discriminatorValue));
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to parse the json data : `{0}` {1}", jsonString, ex);
        }

        int match = 0;
        List<string> matchedTypes = new List<string>();

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(DecoupledFlowHint).GetProperty("AdditionalProperties") == null)
            {
                newGatewayFlowAllOfFlowHint =
                    new GatewayFlowAllOfFlowHint(
                        JsonConvert.DeserializeObject<DecoupledFlowHint>(jsonString, SerializerSettings));
            }
            else
            {
                newGatewayFlowAllOfFlowHint = new GatewayFlowAllOfFlowHint(
                    JsonConvert.DeserializeObject<DecoupledFlowHint>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("DecoupledFlowHint");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into DecoupledFlowHint: {1}", jsonString, exception);
        }

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(RedirectFlowHint).GetProperty("AdditionalProperties") == null)
            {
                newGatewayFlowAllOfFlowHint =
                    new GatewayFlowAllOfFlowHint(
                        JsonConvert.DeserializeObject<RedirectFlowHint>(jsonString, SerializerSettings));
            }
            else
            {
                newGatewayFlowAllOfFlowHint = new GatewayFlowAllOfFlowHint(
                    JsonConvert.DeserializeObject<RedirectFlowHint>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("RedirectFlowHint");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into RedirectFlowHint: {1}", jsonString, exception);
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
        return newGatewayFlowAllOfFlowHint;
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as GatewayFlowAllOfFlowHint);
    }

    /// <summary>
    /// Returns true if GatewayFlowAllOfFlowHint instances are equal
    /// </summary>
    /// <param name="input">Instance of GatewayFlowAllOfFlowHint to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GatewayFlowAllOfFlowHint input)
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
            int hashCode = 41;
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
/// Custom JSON converter for GatewayFlowAllOfFlowHint
/// </summary>
public class GatewayFlowAllOfFlowHintJsonConverter : JsonConverter
{
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue((string)(typeof(GatewayFlowAllOfFlowHint).GetMethod("ToJson").Invoke(value, null)));
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
            return GatewayFlowAllOfFlowHint.FromJson(JObject.Load(reader).ToString(Formatting.None));
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