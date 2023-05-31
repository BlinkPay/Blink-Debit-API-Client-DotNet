using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// Decoupled flow hint.
  /// </summary>
  [DataContract]
  public class DecoupledFlowHint : FlowHint {
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DecoupledFlowHint {\n");
      sb.Append("  IdentifierType: ").Append(IdentifierType).Append("\n");
      sb.Append("  IdentifierValue: ").Append(IdentifierValue).Append("\n");
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
