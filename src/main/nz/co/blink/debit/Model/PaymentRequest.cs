using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The payment request model.
  /// </summary>
  [DataContract]
  public class PaymentRequest {
    /// <summary>
    /// The consent ID
    /// </summary>
    /// <value>The consent ID</value>
    [DataMember(Name="consent_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "consent_id")]
    public Guid? ConsentId { get; set; }

    /// <summary>
    /// Gets or Sets EnduringPayment
    /// </summary>
    [DataMember(Name="enduring_payment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enduring_payment")]
    public EnduringPaymentRequest EnduringPayment { get; set; }

    /// <summary>
    /// The account reference ID from account list. This is required if the account selection information was provided to you on the consents endpoint.
    /// </summary>
    /// <value>The account reference ID from account list. This is required if the account selection information was provided to you on the consents endpoint.</value>
    [DataMember(Name="account_reference_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account_reference_id")]
    public Guid? AccountReferenceId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PaymentRequest {\n");
      sb.Append("  ConsentId: ").Append(ConsentId).Append("\n");
      sb.Append("  EnduringPayment: ").Append(EnduringPayment).Append("\n");
      sb.Append("  AccountReferenceId: ").Append(AccountReferenceId).Append("\n");
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
