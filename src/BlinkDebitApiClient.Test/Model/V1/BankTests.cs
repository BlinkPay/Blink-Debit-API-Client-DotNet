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

using BlinkDebitApiClient.Model.V1;
using Newtonsoft.Json;
using Xunit;

namespace BlinkDebitApiClient.Test.Model.V1;

/// <summary>
/// Unit tests for <see cref="Bank"/> enum wire (JSON) serialisation.
/// The enum is annotated with <c>StringEnumConverter</c>, so the <c>EnumMember</c>
/// value is the on-the-wire representation for both requests and responses.
/// </summary>
public class BankTests
{
    [Theory(DisplayName = "Each Bank enum member serialises to its expected wire value")]
    [InlineData(Bank.ASB, "ASB")]
    [InlineData(Bank.ANZ, "ANZ")]
    [InlineData(Bank.BNZ, "BNZ")]
    [InlineData(Bank.Westpac, "Westpac")]
    [InlineData(Bank.KiwiBank, "Kiwibank")]
    [InlineData(Bank.PNZ, "PNZ")]
    [InlineData(Bank.Cybersource, "Cybersource")]
    public void SerialisesToExpectedWireValue(Bank bank, string expectedWireValue)
    {
        var json = JsonConvert.SerializeObject(bank);

        Assert.Equal($"\"{expectedWireValue}\"", json);
    }

    [Theory(DisplayName = "Each expected wire value deserialises to its Bank enum member")]
    [InlineData("ASB", Bank.ASB)]
    [InlineData("ANZ", Bank.ANZ)]
    [InlineData("BNZ", Bank.BNZ)]
    [InlineData("Westpac", Bank.Westpac)]
    [InlineData("Kiwibank", Bank.KiwiBank)]
    [InlineData("PNZ", Bank.PNZ)]
    [InlineData("Cybersource", Bank.Cybersource)]
    public void DeserialisesFromExpectedWireValue(string wireValue, Bank expectedBank)
    {
        var bank = JsonConvert.DeserializeObject<Bank>($"\"{wireValue}\"");

        Assert.Equal(expectedBank, bank);
    }
}
