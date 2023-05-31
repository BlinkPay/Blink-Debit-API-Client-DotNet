using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for quick payment response.
  /// </summary>
  [DataContract]
  public class QuickPaymentResponse {
    /// <summary>
    /// The quick payment ID.
    /// </summary>
    /// <value>The quick payment ID.</value>
    [DataMember(Name="quick_payment_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "quick_payment_id")]
    public Guid? QuickPaymentId { get; set; }

    /// <summary>
    /// Gets or Sets Consent
    /// </summary>
    [DataMember(Name="consent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "consent")]
    public Consent Consent { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class QuickPaymentResponse {\n");
      sb.Append("  QuickPaymentId: ").Append(QuickPaymentId).Append("\n");
      sb.Append("  Consent: ").Append(Consent).Append("\n");
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
