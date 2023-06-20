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

using System.Collections.Generic;

namespace BlinkDebitApiClient.Config;

/// <summary>
/// <see cref="GlobalConfiguration"/> provides a compile-time extension point for globally configuring
/// API Clients.
/// </summary>
/// <remarks>
/// A customized implementation via partial class may reside in another file and may
/// be excluded from automatic generation via a .openapi-generator-ignore file.
/// </remarks>
public class GlobalConfiguration : Configuration
{
    #region Private Members

    private static readonly object GlobalConfigSync = new { };
    private static IReadableConfiguration _globalConfiguration;

    #endregion Private Members

    #region Constructors

    /// <inheritdoc />
    private GlobalConfiguration()
    {
    }

    /// <inheritdoc />
    public GlobalConfiguration(IDictionary<string, string> defaultHeader, IDictionary<string, string> apiKey,
        IDictionary<string, string> apiKeyPrefix, string basePath = "http://localhost:3000/api") : base(defaultHeader,
        apiKey, apiKeyPrefix, basePath)
    {
    }

    /// <inheritdoc />
    static GlobalConfiguration() => Instance = new GlobalConfiguration();

    #endregion Constructors

    /// <summary>
    /// Gets or sets the default Configuration.
    /// </summary>
    /// <value>Configuration.</value>
    public static IReadableConfiguration Instance
    {
        get => _globalConfiguration;
        private set
        {
            lock (GlobalConfigSync)
            {
                _globalConfiguration = value;
            }
        }
    }
}