using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The payment response
  /// </summary>
  [DataContract]
  public class PaymentResponse {
    /// <summary>
    /// The payment ID
    /// </summary>
    /// <value>The payment ID</value>
    [DataMember(Name="payment_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payment_id")]
    public Guid? PaymentId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PaymentResponse {\n");
      sb.Append("  PaymentId: ").Append(PaymentId).Append("\n");
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
