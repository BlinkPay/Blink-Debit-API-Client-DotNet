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
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The detailed error response.
/// </summary>
[DataContract(Name = "detail-error-response-model")]
public class DetailErrorResponseModel : IEquatable<DetailErrorResponseModel>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DetailErrorResponseModel" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected DetailErrorResponseModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailErrorResponseModel" /> class.
    /// </summary>
    /// <param name="timestamp">The error timestamp..</param>
    /// <param name="status">The status code..</param>
    /// <param name="error">The title of the error code..</param>
    /// <param name="message">The error detail. (required).</param>
    /// <param name="path">The requested path when the error was triggered..</param>
    /// <param name="code">A code supplied by BlinkPay to reference the error type.</param>
    public DetailErrorResponseModel(DateTimeOffset timestamp = default(DateTimeOffset), int status = default(int),
        string error = default(string), string message = default(string), string path = default(string),
        string code = default(string))
    {
        // to ensure "message" is required (not null)
        Message = message ??
                  throw new BlinkInvalidValueException(
                      "message is a required property for DetailErrorResponseModel and cannot be null");
        Timestamp = timestamp;
        Status = status;
        Error = error;
        Path = path;
        Code = code;
    }

    /// <summary>
    /// The error timestamp.
    /// </summary>
    /// <value>The error timestamp.</value>
    [DataMember(Name = "timestamp", EmitDefaultValue = false)]
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// The status code.
    /// </summary>
    /// <value>The status code.</value>
    [DataMember(Name = "status", EmitDefaultValue = false)]
    public int Status { get; set; }

    /// <summary>
    /// The title of the error code.
    /// </summary>
    /// <value>The title of the error code.</value>
    [DataMember(Name = "error", EmitDefaultValue = false)]
    public string Error { get; set; }

    /// <summary>
    /// The error detail.
    /// </summary>
    /// <value>The error detail.</value>
    [DataMember(Name = "message", IsRequired = true, EmitDefaultValue = true)]
    public string Message { get; set; }

    /// <summary>
    /// The requested path when the error was triggered.
    /// </summary>
    /// <value>The requested path when the error was triggered.</value>
    [DataMember(Name = "path", EmitDefaultValue = false)]
    public string Path { get; set; }

    /// <summary>
    /// A code supplied by BlinkPay to reference the error type
    /// </summary>
    /// <value>A code supplied by BlinkPay to reference the error type</value>
    [DataMember(Name = "code", EmitDefaultValue = false)]
    public string Code { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class DetailErrorResponseModel {\n");
        sb.Append("  Timestamp: ").Append(Timestamp).Append('\n');
        sb.Append("  Status: ").Append(Status).Append('\n');
        sb.Append("  Error: ").Append(Error).Append('\n');
        sb.Append("  Message: ").Append(Message).Append('\n');
        sb.Append("  Path: ").Append(Path).Append('\n');
        sb.Append("  Code: ").Append(Code).Append('\n');
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
        return Equals(input as DetailErrorResponseModel);
    }

    /// <summary>
    /// Returns true if DetailErrorResponseModel instances are equal
    /// </summary>
    /// <param name="input">Instance of DetailErrorResponseModel to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(DetailErrorResponseModel input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                Timestamp == input.Timestamp ||
                (Timestamp != null &&
                 Timestamp.Equals(input.Timestamp))
            ) &&
            (
                Status == input.Status ||
                Status.Equals(input.Status)
            ) &&
            (
                Error == input.Error ||
                (Error != null &&
                 Error.Equals(input.Error))
            ) &&
            (
                Message == input.Message ||
                (Message != null &&
                 Message.Equals(input.Message))
            ) &&
            (
                Path == input.Path ||
                (Path != null &&
                 Path.Equals(input.Path))
            ) &&
            (
                Code == input.Code ||
                (Code != null &&
                 Code.Equals(input.Code))
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
            if (Timestamp != null)
            {
                hashCode = (hashCode * 59) + Timestamp.GetHashCode();
            }

            hashCode = (hashCode * 59) + Status.GetHashCode();
            if (Error != null)
            {
                hashCode = (hashCode * 59) + Error.GetHashCode();
            }

            if (Message != null)
            {
                hashCode = (hashCode * 59) + Message.GetHashCode();
            }

            if (Path != null)
            {
                hashCode = (hashCode * 59) + Path.GetHashCode();
            }

            if (Code != null)
            {
                hashCode = (hashCode * 59) + Code.GetHashCode();
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