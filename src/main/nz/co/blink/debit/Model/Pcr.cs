using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// PCR (Particulars, code, reference) details.
  /// </summary>
  [DataContract]
  public class Pcr {
    /// <summary>
    /// The particulars
    /// </summary>
    /// <value>The particulars</value>
    [DataMember(Name="particulars", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "particulars")]
    public string Particulars { get; set; }

    /// <summary>
    /// The code
    /// </summary>
    /// <value>The code</value>
    [DataMember(Name="code", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }

    /// <summary>
    /// The reference
    /// </summary>
    /// <value>The reference</value>
    [DataMember(Name="reference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reference")]
    public string Reference { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Pcr {\n");
      sb.Append("  Particulars: ").Append(Particulars).Append("\n");
      sb.Append("  Code: ").Append(Code).Append("\n");
      sb.Append("  Reference: ").Append(Reference).Append("\n");
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
