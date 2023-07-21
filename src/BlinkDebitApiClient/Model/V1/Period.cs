/*
 * Copyright (c) 2023 BlinkPay
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The available periods selection.  A \&quot;monthly\&quot; period with a from_timestamp 2019-08-21T00:00:00 will have periods defined as - 2019-08-21T00:00:00 to 2019-09-20T23:59:59  - 2019-09-21T00:00:00 to 2019-10-20T23:59:59  - 2019-10-21T00:00:00 to 2019-11-20T23:59:59  - Etc  A \&quot;weekly\&quot; period with a from_timestamp 2019-08-21T00:00:00 will have periods defined as - 2019-08-21T00:00:00 to 2019-08-27T23:59:59  - 2019-08-28T00:00:00 to 2019-09-03T23:59:59  - 2019-09-04T00:00:00 to 2019-09-10T23:59:59  - Etc
/// </summary>
/// <value>The available periods selection.  A \&quot;monthly\&quot; period with a from_timestamp 2019-08-21T00:00:00 will have periods defined as - 2019-08-21T00:00:00 to 2019-09-20T23:59:59  - 2019-09-21T00:00:00 to 2019-10-20T23:59:59  - 2019-10-21T00:00:00 to 2019-11-20T23:59:59  - Etc  A \&quot;weekly\&quot; period with a from_timestamp 2019-08-21T00:00:00 will have periods defined as - 2019-08-21T00:00:00 to 2019-08-27T23:59:59  - 2019-08-28T00:00:00 to 2019-09-03T23:59:59  - 2019-09-04T00:00:00 to 2019-09-10T23:59:59  - Etc</value>
[JsonConverter(typeof(StringEnumConverter))]
public enum Period
{
    /// <summary>
    /// Enum Annual for value: annual
    /// </summary>
    [EnumMember(Value = "annual")] Annual = 1,

    /// <summary>
    /// Enum Daily for value: daily
    /// </summary>
    [EnumMember(Value = "daily")] Daily = 2,

    /// <summary>
    /// Enum Fortnightly for value: fortnightly
    /// </summary>
    [EnumMember(Value = "fortnightly")] Fortnightly = 3,

    /// <summary>
    /// Enum Monthly for value: monthly
    /// </summary>
    [EnumMember(Value = "monthly")] Monthly = 4,

    /// <summary>
    /// Enum Weekly for value: weekly
    /// </summary>
    [EnumMember(Value = "weekly")] Weekly = 5
}