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

namespace BlinkDebitApiClient.Enums;

/// <summary>
/// The enumeration of constants.
/// </summary>
public enum BlinkDebitConstant
{
    REQUEST_ID,
    CORRELATION_ID,
    IDEMPOTENCY_KEY,
    BEARER
}

/// <summary>
/// The extensions of BlinkDebitConstant to retrieve the value based on the enum value as enum types cannot have constructors.
/// </summary>
public static class BlinkDebitConstantExtensions
{
    /// <summary>
    /// Returns the value.
    /// </summary>
    /// <param name="constant"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetValue(this BlinkDebitConstant constant)
    {
        return constant switch
        {
            BlinkDebitConstant.REQUEST_ID => "request-id",
            BlinkDebitConstant.CORRELATION_ID => "x-correlation-id",
            BlinkDebitConstant.IDEMPOTENCY_KEY => "idempotency-key",
            BlinkDebitConstant.BEARER => "Bearer ",
            _ => throw new ArgumentOutOfRangeException(nameof(constant), constant, "Unknown BlinkDebitConstant")
        };
    }
}