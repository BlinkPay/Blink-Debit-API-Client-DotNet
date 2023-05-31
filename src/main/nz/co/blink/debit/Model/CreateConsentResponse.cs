using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for the returned details from a consent, once created.
  /// </summary>
  [DataContract]
  public class CreateConsentResponse {
    /// <summary>
    /// The consent ID
    /// </summary>
    /// <value>The consent ID</value>
    [DataMember(Name="consent_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "consent_id")]
    public Guid? ConsentId { get; set; }

    /// <summary>
    /// The URL to redirect the user to, returned if the flow type is \"redirect\" or \"gateway\"
    /// </summary>
    /// <value>The URL to redirect the user to, returned if the flow type is \"redirect\" or \"gateway\"</value>
    [DataMember(Name="redirect_uri", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "redirect_uri")]
    public string RedirectUri { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CreateConsentResponse {\n");
      sb.Append("  ConsentId: ").Append(ConsentId).Append("\n");
      sb.Append("  RedirectUri: ").Append(RedirectUri).Append("\n");
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
