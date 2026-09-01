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

using BlinkDebitApiClient.Client;
using Xunit;

namespace BlinkDebitApiClient.Test.Client;

/// <summary>
/// Unit tests for <see cref="ClientUtils"/>. Filenames are taken from the server's
/// Content-Disposition header, so the directory must be stripped before the value is
/// used to build a local path.
/// </summary>
public class ClientUtilsTests
{
    [Theory(DisplayName = "A filename is stripped of any directory component")]
    [InlineData("/var/tmp/statement.pdf", "statement.pdf")]
    [InlineData("C:\\Temp\\statement.pdf", "statement.pdf")]
    [InlineData("../../etc/passwd", "passwd")]
    [InlineData("statement.pdf", "statement.pdf")]
    public void StripsDirectoryFromFilename(string filename, string expected)
    {
        Assert.Equal(expected, ClientUtils.SanitizeFilename(filename));
    }
}
