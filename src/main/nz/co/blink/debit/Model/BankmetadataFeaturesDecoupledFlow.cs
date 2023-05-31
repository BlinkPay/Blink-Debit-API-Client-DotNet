using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The decoupled flow bank feature
  /// </summary>
  [DataContract]
  public class BankmetadataFeaturesDecoupledFlow {
    /// <summary>
    /// Whether the Decoupled Flow is enabled.
    /// </summary>
    /// <value>Whether the Decoupled Flow is enabled.</value>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// If enabled, will show the available fields to use to identify the customer with their bank.
    /// </summary>
    /// <value>If enabled, will show the available fields to use to identify the customer with their bank.</value>
    [DataMember(Name="available_identifiers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "available_identifiers")]
    public List<BankmetadataFeaturesDecoupledFlowAvailableIdentifiers> AvailableIdentifiers { get; set; }

    /// <summary>
    /// ISO8601 time duration until the decoupled flow consent request times out
    /// </summary>
    /// <value>ISO8601 time duration until the decoupled flow consent request times out</value>
    [DataMember(Name="request_timeout", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "request_timeout")]
    public string RequestTimeout { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BankmetadataFeaturesDecoupledFlow {\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  AvailableIdentifiers: ").Append(AvailableIdentifiers).Append("\n");
      sb.Append("  RequestTimeout: ").Append(RequestTimeout).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
