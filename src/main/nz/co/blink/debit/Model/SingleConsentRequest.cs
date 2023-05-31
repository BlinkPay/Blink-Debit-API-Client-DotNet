using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for a single consent request, relating to a one-off payment.
  /// </summary>
  [DataContract]
  public class SingleConsentRequest : ConsentDetail {
    /// <summary>
    /// Gets or Sets Flow
    /// </summary>
    [DataMember(Name="flow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flow")]
    public AuthFlow Flow { get; set; }

    /// <summary>
    /// Gets or Sets Pcr
    /// </summary>
    [DataMember(Name="pcr", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pcr")]
    public Pcr Pcr { get; set; }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public Amount Amount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SingleConsentRequest {\n");
      sb.Append("  Flow: ").Append(Flow).Append("\n");
      sb.Append("  Pcr: ").Append(Pcr).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
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
