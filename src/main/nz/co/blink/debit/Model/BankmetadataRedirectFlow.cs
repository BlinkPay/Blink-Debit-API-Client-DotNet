using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// Details about the mandatory redirect flow functionality.
  /// </summary>
  [DataContract]
  public class BankmetadataRedirectFlow {
    /// <summary>
    /// Whether the redirect flow is enabled
    /// </summary>
    /// <value>Whether the redirect flow is enabled</value>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// ISO8601 time duration until the redirect flow consent request times out
    /// </summary>
    /// <value>ISO8601 time duration until the redirect flow consent request times out</value>
    [DataMember(Name="request_timeout", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "request_timeout")]
    public string RequestTimeout { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BankmetadataRedirectFlow {\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
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
