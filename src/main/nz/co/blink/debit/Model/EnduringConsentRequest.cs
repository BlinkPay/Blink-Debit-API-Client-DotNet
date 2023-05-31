using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The model for an enduring consent request, relating to multiple payments.
  /// </summary>
  [DataContract]
  public class EnduringConsentRequest : ConsentDetail {
    /// <summary>
    /// Gets or Sets Flow
    /// </summary>
    [DataMember(Name="flow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flow")]
    public AuthFlow Flow { get; set; }

    /// <summary>
    /// The ISO 8601 start date to calculate the periods for which to calculate the consent period.
    /// </summary>
    /// <value>The ISO 8601 start date to calculate the periods for which to calculate the consent period.</value>
    [DataMember(Name="from_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "from_timestamp")]
    public DateTime? FromTimestamp { get; set; }

    /// <summary>
    /// The ISO 8601 timeout for when an enduring consent will expire. If this field is blank, an indefinite request will be attempted.
    /// </summary>
    /// <value>The ISO 8601 timeout for when an enduring consent will expire. If this field is blank, an indefinite request will be attempted.</value>
    [DataMember(Name="expiry_timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expiry_timestamp")]
    public DateTime? ExpiryTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets Period
    /// </summary>
    [DataMember(Name="period", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "period")]
    public Period Period { get; set; }

    /// <summary>
    /// Gets or Sets MaximumAmountPeriod
    /// </summary>
    [DataMember(Name="maximum_amount_period", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maximum_amount_period")]
    public Amount MaximumAmountPeriod { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EnduringConsentRequest {\n");
      sb.Append("  Flow: ").Append(Flow).Append("\n");
      sb.Append("  FromTimestamp: ").Append(FromTimestamp).Append("\n");
      sb.Append("  ExpiryTimestamp: ").Append(ExpiryTimestamp).Append("\n");
      sb.Append("  Period: ").Append(Period).Append("\n");
      sb.Append("  MaximumAmountPeriod: ").Append(MaximumAmountPeriod).Append("\n");
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
