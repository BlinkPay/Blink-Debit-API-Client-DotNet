using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The PCR and to use in the &#x60;full_refund&#x60; request.
  /// </summary>
  [DataContract]
  public class FullRefundRequest : RefundDetail {
    /// <summary>
    /// Gets or Sets Pcr
    /// </summary>
    [DataMember(Name="pcr", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pcr")]
    public Pcr Pcr { get; set; }

    /// <summary>
    /// The URI that the merchant will need to visit to authorise the refund payment from their bank, if applicable.
    /// </summary>
    /// <value>The URI that the merchant will need to visit to authorise the refund payment from their bank, if applicable.</value>
    [DataMember(Name="consent_redirect", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "consent_redirect")]
    public string ConsentRedirect { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FullRefundRequest {\n");
      sb.Append("  Pcr: ").Append(Pcr).Append("\n");
      sb.Append("  ConsentRedirect: ").Append(ConsentRedirect).Append("\n");
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
