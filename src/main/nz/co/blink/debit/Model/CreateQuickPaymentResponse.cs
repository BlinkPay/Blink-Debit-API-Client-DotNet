using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The quick payment request
  /// </summary>
  [DataContract]
  public class CreateQuickPaymentResponse {
    /// <summary>
    /// The quick payment ID
    /// </summary>
    /// <value>The quick payment ID</value>
    [DataMember(Name="quick_payment_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "quick_payment_id")]
    public Guid? QuickPaymentId { get; set; }

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
      sb.Append("class CreateQuickPaymentResponse {\n");
      sb.Append("  QuickPaymentId: ").Append(QuickPaymentId).Append("\n");
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
