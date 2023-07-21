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
/// The authentication flow detail
/// </summary>
[JsonConverter(typeof(AuthFlowDetailJsonConverter))]
[DataContract(Name = "auth_flow_detail")]
public class AuthFlowDetail : AbstractOpenAPISchema, IEquatable<AuthFlowDetail>, IValidatableObject
{
    /// <summary>
    /// The flow type, i.e. Redirect, Decoupled or Gateway.
    /// </summary>
    /// <value>The flow type, i.e. Redirect, Decoupled or Gateway.</value>
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
        [EnumMember(Value = "decoupled")] Decoupled,

        /// <summary>
        /// Enum Gateway for value: gateway
        /// </summary>
        [EnumMember(Value = "gateway")] Gateway
    }

    /// <summary>
    /// The flow type, i.e. Redirect, Decoupled or Gateway.
    /// </summary>
    /// <value>The flow type, i.e. Redirect, Decoupled or Gateway.</value>
    /// <example>&quot;redirect&quot;</example>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthFlowDetail" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected AuthFlowDetail()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthFlowDetail" /> class
    /// with the <see cref="GatewayFlow" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GatewayFlow.</param>
    public AuthFlowDetail(GatewayFlow actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new BlinkInvalidValueException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthFlowDetail" /> class
    /// with the <see cref="RedirectFlow" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of RedirectFlow.</param>
    public AuthFlowDetail(RedirectFlow actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new BlinkInvalidValueException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthFlowDetail" /> class
    /// with the <see cref="DecoupledFlow" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of DecoupledFlow.</param>
    public AuthFlowDetail(DecoupledFlow actualInstance)
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
            if (value.GetType() == typeof(DecoupledFlow))
            {
                _actualInstance = value;
            }
            else if (value.GetType() == typeof(GatewayFlow))
            {
                _actualInstance = value;
            }
            else if (value.GetType() == typeof(RedirectFlow))
            {
                _actualInstance = value;
            }
            else
            {
                throw new BlinkInvalidValueException(
                    "Invalid instance found. Must be the following types: DecoupledFlow, GatewayFlow, RedirectFlow");
            }
        }
    }

    /// <summary>
    /// Get the actual instance of `GatewayFlow`. If the actual instance is not `GatewayFlow`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of GatewayFlow</returns>
    public GatewayFlow GetGatewayFlow()
    {
        return (GatewayFlow)ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `RedirectFlow`. If the actual instance is not `RedirectFlow`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of RedirectFlow</returns>
    public RedirectFlow GetRedirectFlow()
    {
        return (RedirectFlow)ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `DecoupledFlow`. If the actual instance is not `DecoupledFlow`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of DecoupledFlow</returns>
    public DecoupledFlow GetDecoupledFlow()
    {
        return (DecoupledFlow)ActualInstance;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class AuthFlowDetail {\n");
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
    /// Converts the JSON string into an instance of AuthFlowDetail
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of AuthFlowDetail</returns>
    public static AuthFlowDetail FromJson(string jsonString)
    {
        AuthFlowDetail newAuthFlowDetail = null;

        if (string.IsNullOrEmpty(jsonString))
        {
            return newAuthFlowDetail;
        }

        try
        {
            var discriminatorObj = JObject.Parse(jsonString)["type"];
            var discriminatorValue = discriminatorObj == null ? string.Empty : discriminatorObj.ToString();
            switch (discriminatorValue)
            {
                case "decoupled":
                    newAuthFlowDetail =
                        new AuthFlowDetail(JsonConvert.DeserializeObject<DecoupledFlow>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newAuthFlowDetail;
                case "gateway":
                    newAuthFlowDetail =
                        new AuthFlowDetail(JsonConvert.DeserializeObject<GatewayFlow>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newAuthFlowDetail;
                case "redirect":
                    newAuthFlowDetail =
                        new AuthFlowDetail(JsonConvert.DeserializeObject<RedirectFlow>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newAuthFlowDetail;
                default:
                    Debug.WriteLine(
                        $"Failed to lookup discriminator value `{discriminatorValue}` for AuthFlowDetail. Possible values: decoupled-flow gateway-flow redirect-flow");
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
            if (typeof(DecoupledFlow).GetProperty("AdditionalProperties") == null)
            {
                newAuthFlowDetail =
                    new AuthFlowDetail(JsonConvert.DeserializeObject<DecoupledFlow>(jsonString, SerializerSettings));
            }
            else
            {
                newAuthFlowDetail =
                    new AuthFlowDetail(JsonConvert.DeserializeObject<DecoupledFlow>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("DecoupledFlow");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into DecoupledFlow: {1}", jsonString, exception);
        }

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(GatewayFlow).GetProperty("AdditionalProperties") == null)
            {
                newAuthFlowDetail =
                    new AuthFlowDetail(JsonConvert.DeserializeObject<GatewayFlow>(jsonString, SerializerSettings));
            }
            else
            {
                newAuthFlowDetail =
                    new AuthFlowDetail(JsonConvert.DeserializeObject<GatewayFlow>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("GatewayFlow");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into GatewayFlow: {1}", jsonString, exception);
        }

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(RedirectFlow).GetProperty("AdditionalProperties") == null)
            {
                newAuthFlowDetail =
                    new AuthFlowDetail(JsonConvert.DeserializeObject<RedirectFlow>(jsonString, SerializerSettings));
            }
            else
            {
                newAuthFlowDetail =
                    new AuthFlowDetail(JsonConvert.DeserializeObject<RedirectFlow>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("RedirectFlow");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into RedirectFlow: {1}", jsonString, exception);
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
        return newAuthFlowDetail;
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as AuthFlowDetail);
    }

    /// <summary>
    /// Returns true if AuthFlowDetail instances are equal
    /// </summary>
    /// <param name="input">Instance of AuthFlowDetail to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AuthFlowDetail input)
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
/// Custom JSON converter for AuthFlowDetail
/// </summary>
public class AuthFlowDetailJsonConverter : JsonConverter
{
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue((string)(typeof(AuthFlowDetail).GetMethod("ToJson").Invoke(value, null)));
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
            return AuthFlowDetail.FromJson(JObject.Load(reader).ToString(Formatting.None));
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