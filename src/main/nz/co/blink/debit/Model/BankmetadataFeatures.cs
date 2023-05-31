using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The available bank features
  /// </summary>
  [DataContract]
  public class BankmetadataFeatures {
    /// <summary>
    /// Gets or Sets EnduringConsent
    /// </summary>
    [DataMember(Name="enduring_consent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enduring_consent")]
    public BankmetadataFeaturesEnduringConsent EnduringConsent { get; set; }

    /// <summary>
    /// Gets or Sets DecoupledFlow
    /// </summary>
    [DataMember(Name="decoupled_flow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "decoupled_flow")]
    public BankmetadataFeaturesDecoupledFlow DecoupledFlow { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BankmetadataFeatures {\n");
      sb.Append("  EnduringConsent: ").Append(EnduringConsent).Append("\n");
      sb.Append("  DecoupledFlow: ").Append(DecoupledFlow).Append("\n");
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
