using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The details for a Decoupled flow.
  /// </summary>
  [DataContract]
  public class DecoupledFlow : AuthFlowDetail {
    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name="bank", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "bank")]
    public Bank Bank { get; set; }

    /// <summary>
    /// Gets or Sets IdentifierType
    /// </summary>
    [DataMember(Name="identifier_type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "identifier_type")]
    public IdentifierType IdentifierType { get; set; }

    /// <summary>
    /// Gets or Sets IdentifierValue
    /// </summary>
    [DataMember(Name="identifier_value", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "identifier_value")]
    public string IdentifierValue { get; set; }

    /// <summary>
    /// A callback URL to call once the consent status has been updated using decoupled flow. Blink will also append the `cid` (the Consent ID) in an additional URL parameter. This is sent to your api as a GET request and will be retried up to 3 times if 5xx errors are received from your server.
    /// </summary>
    /// <value>A callback URL to call once the consent status has been updated using decoupled flow. Blink will also append the `cid` (the Consent ID) in an additional URL parameter. This is sent to your api as a GET request and will be retried up to 3 times if 5xx errors are received from your server.</value>
    [DataMember(Name="callback_url", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "callback_url")]
    public string CallbackUrl { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DecoupledFlow {\n");
      sb.Append("  Bank: ").Append(Bank).Append("\n");
      sb.Append("  IdentifierType: ").Append(IdentifierType).Append("\n");
      sb.Append("  IdentifierValue: ").Append(IdentifierValue).Append("\n");
      sb.Append("  CallbackUrl: ").Append(CallbackUrl).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public  new string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
