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
using Newtonsoft.Json.Linq;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The refund request model.
/// </summary>
[JsonConverter(typeof(RefundRequestJsonConverter))]
[DataContract(Name = "refund-request")]
public class RefundRequest : AbstractOpenAPISchema, IEquatable<RefundRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefundRequest" /> class
    /// with the <see cref="AccountNumberRefundRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of AccountNumberRefundRequest.</param>
    public RefundRequest(AccountNumberRefundRequest actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new BlinkInvalidValueException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefundRequest" /> class
    /// with the <see cref="PartialRefundRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of PartialRefundRequest.</param>
    public RefundRequest(PartialRefundRequest actualInstance)
    {
        IsNullable = false;
        SchemaType = "oneOf";
        ActualInstance = actualInstance ?? throw new BlinkInvalidValueException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefundRequest" /> class
    /// with the <see cref="FullRefundRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FullRefundRequest.</param>
    public RefundRequest(FullRefundRequest actualInstance)
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
            if (value.GetType() == typeof(AccountNumberRefundRequest))
            {
                _actualInstance = value;
            }
            else if (value.GetType() == typeof(FullRefundRequest))
            {
                _actualInstance = value;
            }
            else if (value.GetType() == typeof(PartialRefundRequest))
            {
                _actualInstance = value;
            }
            else
            {
                throw new BlinkInvalidValueException(
                    "Invalid instance found. Must be the following types: AccountNumberRefundRequest, FullRefundRequest, PartialRefundRequest");
            }
        }
    }

    /// <summary>
    /// Get the actual instance of `AccountNumberRefundRequest`. If the actual instance is not `AccountNumberRefundRequest`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of AccountNumberRefundRequest</returns>
    public AccountNumberRefundRequest GetAccountNumberRefundRequest()
    {
        return (AccountNumberRefundRequest)ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `PartialRefundRequest`. If the actual instance is not `PartialRefundRequest`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of PartialRefundRequest</returns>
    public PartialRefundRequest GetPartialRefundRequest()
    {
        return (PartialRefundRequest)ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `FullRefundRequest`. If the actual instance is not `FullRefundRequest`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of FullRefundRequest</returns>
    public FullRefundRequest GetFullRefundRequest()
    {
        return (FullRefundRequest)ActualInstance;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class RefundRequest {\n");
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
    /// Converts the JSON string into an instance of RefundRequest
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of RefundRequest</returns>
    public static RefundRequest FromJson(string jsonString)
    {
        RefundRequest newRefundRequest = null;

        if (string.IsNullOrEmpty(jsonString))
        {
            return newRefundRequest;
        }

        try
        {
            var discriminatorObj = JObject.Parse(jsonString)["type"];
            var discriminatorValue = discriminatorObj == null ? string.Empty : discriminatorObj.ToString();
            switch (discriminatorValue)
            {
                case "account_number":
                    newRefundRequest =
                        new RefundRequest(JsonConvert.DeserializeObject<AccountNumberRefundRequest>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newRefundRequest;
                case "full_refund":
                    newRefundRequest =
                        new RefundRequest(JsonConvert.DeserializeObject<FullRefundRequest>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newRefundRequest;
                case "partial_refund":
                    newRefundRequest =
                        new RefundRequest(JsonConvert.DeserializeObject<PartialRefundRequest>(jsonString,
                            AdditionalPropertiesSerializerSettings));
                    return newRefundRequest;
                default:
                    Debug.WriteLine(
                        $"Failed to lookup discriminator value `{discriminatorValue}` for RefundRequest. Possible values: account-number-refund-request full-refund-request partial-refund-request");
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
            if (typeof(AccountNumberRefundRequest).GetProperty("AdditionalProperties") == null)
            {
                newRefundRequest =
                    new RefundRequest(
                        JsonConvert.DeserializeObject<AccountNumberRefundRequest>(jsonString, SerializerSettings));
            }
            else
            {
                newRefundRequest =
                    new RefundRequest(JsonConvert.DeserializeObject<AccountNumberRefundRequest>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("AccountNumberRefundRequest");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into AccountNumberRefundRequest: {1}", jsonString, exception);
        }

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(FullRefundRequest).GetProperty("AdditionalProperties") == null)
            {
                newRefundRequest =
                    new RefundRequest(JsonConvert.DeserializeObject<FullRefundRequest>(jsonString, SerializerSettings));
            }
            else
            {
                newRefundRequest =
                    new RefundRequest(JsonConvert.DeserializeObject<FullRefundRequest>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("FullRefundRequest");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into FullRefundRequest: {1}", jsonString, exception);
        }

        try
        {
            // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
            if (typeof(PartialRefundRequest).GetProperty("AdditionalProperties") == null)
            {
                newRefundRequest =
                    new RefundRequest(
                        JsonConvert.DeserializeObject<PartialRefundRequest>(jsonString, SerializerSettings));
            }
            else
            {
                newRefundRequest =
                    new RefundRequest(JsonConvert.DeserializeObject<PartialRefundRequest>(jsonString,
                        AdditionalPropertiesSerializerSettings));
            }

            matchedTypes.Add("PartialRefundRequest");
            match++;
        }
        catch (Exception exception)
        {
            // deserialization failed, try the next one
            Debug.WriteLine("Failed to deserialize `{0}` into PartialRefundRequest: {1}", jsonString, exception);
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
        return newRefundRequest;
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as RefundRequest);
    }

    /// <summary>
    /// Returns true if RefundRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of RefundRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RefundRequest input)
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
/// Custom JSON converter for RefundRequest
/// </summary>
public class RefundRequestJsonConverter : JsonConverter
{
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue((string)(typeof(RefundRequest).GetMethod("ToJson").Invoke(value, null)));
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
            return RefundRequest.FromJson(JObject.Load(reader).ToString(Formatting.None));
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