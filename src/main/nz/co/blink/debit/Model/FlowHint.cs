using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// Optionally include a hint to the Gateway of which flow should be used, allowing the customers details to be prefilled in order to make the checkout experience faster. You can also use Gateway flow hint to instruct Gateway to identify a customer using their last consent ID for mobile payments.
  /// </summary>
  [DataContract]
  public class FlowHint {
    /// <summary>
    /// The flow hint type, i.e. Redirect or Decoupled.
    /// </summary>
    /// <value>The flow hint type, i.e. Redirect or Decoupled.</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name="bank", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "bank")]
    public Bank Bank { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FlowHint {\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Bank: ").Append(Bank).Append("\n");
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
