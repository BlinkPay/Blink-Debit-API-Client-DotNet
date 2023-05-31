using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The enduring consent bank feature
  /// </summary>
  [DataContract]
  public class BankmetadataFeaturesEnduringConsent {
    /// <summary>
    /// If enduring consent is disabled, only single payment consents can be issued.
    /// </summary>
    /// <value>If enduring consent is disabled, only single payment consents can be issued.</value>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// ISO8601 time duration for the maximum allowed enduring consent period, i.e. how long the consent could be used to execute payments for. Appears only if consent_indefinite is false
    /// </summary>
    /// <value>ISO8601 time duration for the maximum allowed enduring consent period, i.e. how long the consent could be used to execute payments for. Appears only if consent_indefinite is false</value>
    [DataMember(Name="maximum_consent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maximum_consent")]
    public string MaximumConsent { get; set; }

    /// <summary>
    /// If the consenting period for payments is indefinite or time-limited by the bank
    /// </summary>
    /// <value>If the consenting period for payments is indefinite or time-limited by the bank</value>
    [DataMember(Name="consent_indefinite", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "consent_indefinite")]
    public bool? ConsentIndefinite { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BankmetadataFeaturesEnduringConsent {\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  MaximumConsent: ").Append(MaximumConsent).Append("\n");
      sb.Append("  ConsentIndefinite: ").Append(ConsentIndefinite).Append("\n");
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
