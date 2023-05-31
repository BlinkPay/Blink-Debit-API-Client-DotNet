using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// Information about a banks enabled features.
  /// </summary>
  [DataContract]
  public class BankMetadata {
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public Bank Name { get; set; }

    /// <summary>
    /// Gets or Sets Features
    /// </summary>
    [DataMember(Name="features", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "features")]
    public BankmetadataFeatures Features { get; set; }

    /// <summary>
    /// Gets or Sets RedirectFlow
    /// </summary>
    [DataMember(Name="redirect_flow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "redirect_flow")]
    public BankmetadataRedirectFlow RedirectFlow { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BankMetadata {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Features: ").Append(Features).Append("\n");
      sb.Append("  RedirectFlow: ").Append(RedirectFlow).Append("\n");
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
