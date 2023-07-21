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

using System.Threading;
using System.Threading.Tasks;
using BlinkDebitApiClient.Config;

namespace BlinkDebitApiClient.Client;

/// <summary>
/// Contract for Asynchronous RESTful API interactions.
///
/// This interface allows consumers to provide a custom API accessor client.
/// </summary>
public interface IAsynchronousClient
{
    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the GET http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> GetAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the POST http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> PostAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the PUT http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> PutAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the DELETE http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> DeleteAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the HEAD http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> HeadAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the OPTIONS http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> OptionsAsync<T>(string path, RequestOptions options,
        IReadableConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Executes a non-blocking call to some <paramref name="path"/> using the PATCH http verb.
    /// </summary>
    /// <param name="path">The relative path to invoke.</param>
    /// <param name="options">The request parameters to pass along to the client.</param>
    /// <param name="configuration">Per-request configurable settings.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A task eventually representing the response data, decorated with <see cref="ApiResponse{T}"/></returns>
    Task<ApiResponse<T>> PatchAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null,
        CancellationToken cancellationToken = default(CancellationToken));
}