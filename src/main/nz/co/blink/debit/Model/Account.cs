using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for a Westpac account.
  /// </summary>
  [DataContract]
  public class Account {
    /// <summary>
    /// The account reference ID from account list. This is required if the account selection information was provided to you on the consents endpoint.
    /// </summary>
    /// <value>The account reference ID from account list. This is required if the account selection information was provided to you on the consents endpoint.</value>
    [DataMember(Name="account_reference_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account_reference_id")]
    public Guid? AccountReferenceId { get; set; }

    /// <summary>
    /// The account number.
    /// </summary>
    /// <value>The account number.</value>
    [DataMember(Name="account_number", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// The account name.
    /// </summary>
    /// <value>The account name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The available balance.
    /// </summary>
    /// <value>The available balance.</value>
    [DataMember(Name="available_balance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "available_balance")]
    public string AvailableBalance { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Account {\n");
      sb.Append("  AccountReferenceId: ").Append(AccountReferenceId).Append("\n");
      sb.Append("  AccountNumber: ").Append(AccountNumber).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  AvailableBalance: ").Append(AvailableBalance).Append("\n");
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
