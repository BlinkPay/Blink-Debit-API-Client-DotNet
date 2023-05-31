using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// The available periods selection.  A \&quot;monthly\&quot; period with a from_timestamp 2019-08-21T00:00:00 will have periods defined as - 2019-08-21T00:00:00 to 2019-09-20T23:59:59  - 2019-09-21T00:00:00 to 2019-10-20T23:59:59  - 2019-10-21T00:00:00 to 2019-11-20T23:59:59  - Etc  A \&quot;weekly\&quot; period with a from_timestamp 2019-08-21T00:00:00 will have periods defined as - 2019-08-21T00:00:00 to 2019-08-27T23:59:59  - 2019-08-28T00:00:00 to 2019-09-03T23:59:59  - 2019-09-04T00:00:00 to 2019-09-10T23:59:59  - Etc
  /// </summary>
  [DataContract]
  public class Period {

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Period {\n");
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
