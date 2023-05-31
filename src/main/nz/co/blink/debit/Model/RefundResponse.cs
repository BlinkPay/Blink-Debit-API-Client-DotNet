using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The refund response
  /// </summary>
  [DataContract]
  public class RefundResponse {
    /// <summary>
    /// The created refund request ID
    /// </summary>
    /// <value>The created refund request ID</value>
    [DataMember(Name="refund_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refund_id")]
    public Guid? RefundId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RefundResponse {\n");
      sb.Append("  RefundId: ").Append(RefundId).Append("\n");
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
