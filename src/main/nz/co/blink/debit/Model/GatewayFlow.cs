using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The details for a Gateway flow.
  /// </summary>
  [DataContract]
  public class GatewayFlow : AuthFlowDetail {
    /// <summary>
    /// The URL to redirect back to once the payment is completed through the gateway. The `cid` (Consent ID) will be added as a URL parameter. If there is an error, an `error` parameter will be appended also.
    /// </summary>
    /// <value>The URL to redirect back to once the payment is completed through the gateway. The `cid` (Consent ID) will be added as a URL parameter. If there is an error, an `error` parameter will be appended also.</value>
    [DataMember(Name="redirect_uri", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "redirect_uri")]
    public string RedirectUri { get; set; }

    /// <summary>
    /// The gateway flow hint
    /// </summary>
    /// <value>The gateway flow hint</value>
    [DataMember(Name="flow_hint", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flow_hint")]
    public Object FlowHint { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class GatewayFlow {\n");
      sb.Append("  RedirectUri: ").Append(RedirectUri).Append("\n");
      sb.Append("  FlowHint: ").Append(FlowHint).Append("\n");
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
