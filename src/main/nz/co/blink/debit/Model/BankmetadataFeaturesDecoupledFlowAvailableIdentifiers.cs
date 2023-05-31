using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class BankmetadataFeaturesDecoupledFlowAvailableIdentifiers {
    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public IdentifierType Type { get; set; }

    /// <summary>
    /// A regex that can be used for validation of the field
    /// </summary>
    /// <value>A regex that can be used for validation of the field</value>
    [DataMember(Name="regex", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "regex")]
    public string Regex { get; set; }

    /// <summary>
    /// The common name of the field
    /// </summary>
    /// <value>The common name of the field</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The description of the field and/or where to find it
    /// </summary>
    /// <value>The description of the field and/or where to find it</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BankmetadataFeaturesDecoupledFlowAvailableIdentifiers {\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Regex: ").Append(Regex).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
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
