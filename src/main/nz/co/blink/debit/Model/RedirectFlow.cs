using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The details for a Redirect flow.
  /// </summary>
  [DataContract]
  public class RedirectFlow : AuthFlowDetail {
    /// <summary>
    /// The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The `cid` (Consent ID) will be added as a URL parameter. If there is an error, an `error` parameter will be appended also.
    /// </summary>
    /// <value>The URI to redirect back to once the consent is completed. App-based workflows may use deep/universal links. The `cid` (Consent ID) will be added as a URL parameter. If there is an error, an `error` parameter will be appended also.</value>
    [DataMember(Name="redirect_uri", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "redirect_uri")]
    public string RedirectUri { get; set; }

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
      sb.Append("class RedirectFlow {\n");
      sb.Append("  RedirectUri: ").Append(RedirectUri).Append("\n");
      sb.Append("  Bank: ").Append(Bank).Append("\n");
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
