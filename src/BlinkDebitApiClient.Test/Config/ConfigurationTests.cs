/*
 * Copyright (c) 2025 BlinkPay
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

using System.IO;
using BlinkDebitApiClient.Config;
using Xunit;

namespace BlinkDebitApiClient.Test.Config;

/// <summary>
/// Unit tests for <see cref="Configuration"/>'s temporary folder handling. Downloaded files
/// are written here, so the default must be a directory private to the current user rather
/// than the publicly writable system temporary folder.
/// </summary>
public class ConfigurationTests
{
    [Theory(DisplayName = "An unset temporary folder path falls back to a private default")]
    [InlineData("")]
    [InlineData(null)]
    public void FallsBackToPrivateDefault(string? tempFolderPath)
    {
        var configuration = new Configuration { TempFolderPath = tempFolderPath };

        Assert.False(string.IsNullOrEmpty(configuration.TempFolderPath));
        Assert.True(Directory.Exists(configuration.TempFolderPath));
        Assert.NotEqual(Path.GetTempPath(), configuration.TempFolderPath);
        Assert.EndsWith(Path.DirectorySeparatorChar.ToString(), configuration.TempFolderPath);
    }

    [Fact(DisplayName = "An explicit temporary folder path is created and separator-terminated")]
    public void CreatesExplicitTemporaryFolder()
    {
        var requested = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            var configuration = new Configuration { TempFolderPath = requested };

            Assert.True(Directory.Exists(requested));
            Assert.Equal(requested + Path.DirectorySeparatorChar, configuration.TempFolderPath);
        }
        finally
        {
            Directory.Delete(requested, true);
        }
    }
}
