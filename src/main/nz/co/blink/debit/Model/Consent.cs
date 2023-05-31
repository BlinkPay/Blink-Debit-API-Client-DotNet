using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for a consent.
  /// </summary>
  [DataContract]
  public class Consent {
    /// <summary>
    /// The consent ID
    /// </summary>
    /// <value>The consent ID</value>
    [DataMember(Name="consent_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "consent_id")]
    public Guid? ConsentId { get; set; }

    /// <summary>
    /// The status of the consent
    /// </summary>
    /// <value>The status of the consent</value>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// The timestamp that the consent was created
    /// </summary>
    /// <value>The timestamp that the consent was created</value>
    [DataMember(Name="creation_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creation_timestamp")]
    public DateTime? CreationTimestamp { get; set; }

    /// <summary>
    /// The time that the status was last updated
    /// </summary>
    /// <value>The time that the status was last updated</value>
    [DataMember(Name="status_updated_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status_updated_timestamp")]
    public DateTime? StatusUpdatedTimestamp { get; set; }

    /// <summary>
    /// The consent details
    /// </summary>
    /// <value>The consent details</value>
    [DataMember(Name="detail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "detail")]
    public OneOfconsentDetail Detail { get; set; }

    /// <summary>
    /// If applicable, the Westpac account list for account selection. If this is included, the merchant is required to ask the customer which account they would like to debit the payment from using the information provided.
    /// </summary>
    /// <value>If applicable, the Westpac account list for account selection. If this is included, the merchant is required to ask the customer which account they would like to debit the payment from using the information provided.</value>
    [DataMember(Name="accounts", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accounts")]
    public List<Account> Accounts { get; set; }

    /// <summary>
    /// Payments associated with this consent, if any.
    /// </summary>
    /// <value>Payments associated with this consent, if any.</value>
    [DataMember(Name="payments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payments")]
    public List<Payment> Payments { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Consent {\n");
      sb.Append("  ConsentId: ").Append(ConsentId).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  CreationTimestamp: ").Append(CreationTimestamp).Append("\n");
      sb.Append("  StatusUpdatedTimestamp: ").Append(StatusUpdatedTimestamp).Append("\n");
      sb.Append("  Detail: ").Append(Detail).Append("\n");
      sb.Append("  Accounts: ").Append(Accounts).Append("\n");
      sb.Append("  Payments: ").Append(Payments).Append("\n");
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
