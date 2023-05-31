using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for a refund.
  /// </summary>
  [DataContract]
  public class Refund {
    /// <summary>
    /// The refund ID.
    /// </summary>
    /// <value>The refund ID.</value>
    [DataMember(Name="refund_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refund_id")]
    public Guid? RefundId { get; set; }

    /// <summary>
    /// The refund status
    /// </summary>
    /// <value>The refund status</value>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// The time that the refund was created.
    /// </summary>
    /// <value>The time that the refund was created.</value>
    [DataMember(Name="creation_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creation_timestamp")]
    public DateTime? CreationTimestamp { get; set; }

    /// <summary>
    /// The time that the status was last updated.
    /// </summary>
    /// <value>The time that the status was last updated.</value>
    [DataMember(Name="status_updated_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status_updated_timestamp")]
    public DateTime? StatusUpdatedTimestamp { get; set; }

    /// <summary>
    /// The customer account number used or to be used for the refund.
    /// </summary>
    /// <value>The customer account number used or to be used for the refund.</value>
    [DataMember(Name="account_number", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or Sets Detail
    /// </summary>
    [DataMember(Name="detail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "detail")]
    public RefundRequest Detail { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Refund {\n");
      sb.Append("  RefundId: ").Append(RefundId).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  CreationTimestamp: ").Append(CreationTimestamp).Append("\n");
      sb.Append("  StatusUpdatedTimestamp: ").Append(StatusUpdatedTimestamp).Append("\n");
      sb.Append("  AccountNumber: ").Append(AccountNumber).Append("\n");
      sb.Append("  Detail: ").Append(Detail).Append("\n");
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
