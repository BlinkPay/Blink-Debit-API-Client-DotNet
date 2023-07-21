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

using BlinkDebitApiClient.Config;

namespace BlinkDebitApiClient.Client;

/// <summary>
/// Contract for Synchronous RESTful API interactions.
///
/// This interface allows consumers to provide a custom API accessor client.
/// </summary>
public interface ISynchronousClient
{
    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the GET http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Get<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);

    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the POST http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Post<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);

    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the PUT http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Put<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);

    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the DELETE http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Delete<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);

    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the HEAD http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Head<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);

    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the OPTIONS http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Options<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);

    /// <summary>
    /// Executes a blocking call to some <paramref name="path"/> using the PATCH http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>The response data, decorated with <see cref="ApiResponse{T}"/></returns>
    ApiResponse<T> Patch<T>(string path, RequestOptions options, IReadableConfiguration configuration = null);
}