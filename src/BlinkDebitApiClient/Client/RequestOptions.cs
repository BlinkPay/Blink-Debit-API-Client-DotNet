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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BlinkDebitApiClient.Client;

/// <summary>
/// A container for generalized request inputs. This type allows consumers to extend the request functionality
/// by abstracting away from the default (built-in) request framework (e.g. RestSharp).
/// </summary>
public class RequestOptions
{
    /// <summary>
    /// Parameters to be bound to path parts of the Request's URL
    /// </summary>
    public Dictionary<string, string> PathParameters { get; set; }

    /// <summary>
    /// Query parameters to be applied to the request.
    /// Keys may have 1 or more values associated.
    /// </summary>
    public Multimap<string, string> QueryParameters { get; set; }

    /// <summary>
    /// Header parameters to be applied to the request.
    /// Keys may have 1 or more values associated.
    /// </summary>
    public Multimap<string, string> HeaderParameters { get; set; }

    /// <summary>
    /// Form parameters to be sent along with the request.
    /// </summary>
    public Dictionary<string, string> FormParameters { get; set; }

    /// <summary>
    /// File parameters to be sent along with the request.
    /// </summary>
    public Multimap<string, Stream> FileParameters { get; set; }

    /// <summary>
    /// Cookies to be sent along with the request.
    /// </summary>
    public List<Cookie> Cookies { get; set; }

    /// <summary>
    /// Operation associated with the request path.
    /// </summary>
    public string Operation { get; set; }

    /// <summary>
    /// Index associated with the operation.
    /// </summary>
    public int OperationIndex { get; set; }

    /// <summary>
    /// Any data associated with a request body.
    /// </summary>
    public Object Data { get; set; }

    /// <summary>
    /// If request should be authenticated with OAuth.
    /// </summary>
    public bool OAuth { get; set; }

    /// <summary>
    /// Constructs a new instance of <see cref="RequestOptions"/>
    /// </summary>
    public RequestOptions()
    {
        PathParameters = new Dictionary<string, string>();
        QueryParameters = new Multimap<string, string>();
        HeaderParameters = new Multimap<string, string>();
        FormParameters = new Dictionary<string, string>();
        FileParameters = new Multimap<string, Stream>();
        Cookies = new List<Cookie>();
    }
}