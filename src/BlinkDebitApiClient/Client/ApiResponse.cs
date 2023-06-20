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
using System.Net;

namespace BlinkDebitApiClient.Client;

/// <summary>
/// Provides a non-generic contract for the ApiResponse wrapper.
/// </summary>
public interface IApiResponse
{
    /// <summary>
    /// The data type of <see cref="Content"/>
    /// </summary>
    Type ResponseType { get; }

    /// <summary>
    /// The content of this response
    /// </summary>
    Object Content { get; }

    /// <summary>
    /// Gets or sets the status code (HTTP status code)
    /// </summary>
    /// <value>The status code.</value>
    HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets or sets the HTTP headers
    /// </summary>
    /// <value>HTTP headers</value>
    Multimap<string, string> Headers { get; }

    /// <summary>
    /// Gets or sets any error text defined by the calling client.
    /// </summary>
    string ErrorText { get; set; }

    /// <summary>
    /// Gets or sets any cookies passed along on the response.
    /// </summary>
    List<Cookie> Cookies { get; set; }

    /// <summary>
    /// The raw content of this response
    /// </summary>
    string RawContent { get; }
}

/// <summary>
/// API Response
/// </summary>
public class ApiResponse<T> : IApiResponse
{
    #region Properties

    /// <summary>
    /// Gets or sets the status code (HTTP status code)
    /// </summary>
    /// <value>The status code.</value>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets or sets the HTTP headers
    /// </summary>
    /// <value>HTTP headers</value>
    public Multimap<string, string> Headers { get; }

    /// <summary>
    /// Gets or sets the data (parsed HTTP body)
    /// </summary>
    /// <value>The data.</value>
    public T Data { get; }

    /// <summary>
    /// Gets or sets any error text defined by the calling client.
    /// </summary>
    public string ErrorText { get; set; }

    /// <summary>
    /// Gets or sets any cookies passed along on the response.
    /// </summary>
    public List<Cookie> Cookies { get; set; }

    /// <summary>
    /// The content of this response
    /// </summary>
    public Type ResponseType => typeof(T);

    /// <summary>
    /// The data type of <see cref="Content"/>
    /// </summary>
    public object Content => Data;

    /// <summary>
    /// The raw content
    /// </summary>
    public string RawContent { get; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}" /> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="headers">HTTP headers.</param>
    /// <param name="data">Data (parsed HTTP body)</param>
    /// <param name="rawContent">Raw content.</param>
    public ApiResponse(HttpStatusCode statusCode, Multimap<string, string> headers, T data, string rawContent)
    {
        StatusCode = statusCode;
        Headers = headers;
        Data = data;
        RawContent = rawContent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}" /> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="headers">HTTP headers.</param>
    /// <param name="data">Data (parsed HTTP body)</param>
    public ApiResponse(HttpStatusCode statusCode, Multimap<string, string> headers, T data) : this(statusCode, headers,
        data, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}" /> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="data">Data (parsed HTTP body)</param>
    /// <param name="rawContent">Raw content.</param>
    public ApiResponse(HttpStatusCode statusCode, T data, string rawContent) : this(statusCode, null, data, rawContent)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}" /> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="data">Data (parsed HTTP body)</param>
    public ApiResponse(HttpStatusCode statusCode, T data) : this(statusCode, data, null)
    {
    }

    #endregion Constructors
}