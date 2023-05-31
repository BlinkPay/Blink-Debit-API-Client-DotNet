using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for a payment.
  /// </summary>
  [DataContract]
  public class Payment {
    /// <summary>
    /// The payment ID
    /// </summary>
    /// <value>The payment ID</value>
    [DataMember(Name="payment_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payment_id")]
    public Guid? PaymentId { get; set; }

    /// <summary>
    /// The type of payment (single or enduring).
    /// </summary>
    /// <value>The type of payment (single or enduring).</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// The status of the payment.
    /// </summary>
    /// <value>The status of the payment.</value>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// The reason for `AcceptedSettlementCompleted`.
    /// </summary>
    /// <value>The reason for `AcceptedSettlementCompleted`.</value>
    [DataMember(Name="accepted_reason", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accepted_reason")]
    public string AcceptedReason { get; set; }

    /// <summary>
    /// The timestamp that the payment was created.
    /// </summary>
    /// <value>The timestamp that the payment was created.</value>
    [DataMember(Name="creation_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creation_timestamp")]
    public DateTime? CreationTimestamp { get; set; }

    /// <summary>
    /// The timestamp that the payment status was last updated.
    /// </summary>
    /// <value>The timestamp that the payment status was last updated.</value>
    [DataMember(Name="status_updated_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status_updated_timestamp")]
    public DateTime? StatusUpdatedTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets Detail
    /// </summary>
    [DataMember(Name="detail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "detail")]
    public PaymentRequest Detail { get; set; }

    /// <summary>
    /// Refunds that are related to this payment, if any.
    /// </summary>
    /// <value>Refunds that are related to this payment, if any.</value>
    [DataMember(Name="refunds", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refunds")]
    public List<Refund> Refunds { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Payment {\n");
      sb.Append("  PaymentId: ").Append(PaymentId).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  AcceptedReason: ").Append(AcceptedReason).Append("\n");
      sb.Append("  CreationTimestamp: ").Append(CreationTimestamp).Append("\n");
      sb.Append("  StatusUpdatedTimestamp: ").Append(StatusUpdatedTimestamp).Append("\n");
      sb.Append("  Detail: ").Append(Detail).Append("\n");
      sb.Append("  Refunds: ").Append(Refunds).Append("\n");
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
