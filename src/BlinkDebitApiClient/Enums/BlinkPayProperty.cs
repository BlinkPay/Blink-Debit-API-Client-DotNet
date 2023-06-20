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
/// The enumeration of Blink Pay properties.
/// </summary>
public enum BlinkPayProperty
{
    BLINKPAY_DEBIT_URL,
    BLINKPAY_CLIENT_ID,
    BLINKPAY_CLIENT_SECRET,
    BLINKPAY_MAX_CONNECTIONS,
    BLINKPAY_MAX_IDLE_TIME,
    BLINKPAY_MAX_LIFE_TIME,
    BLINKPAY_PENDING_ACQUIRE_TIMEOUT,
    BLINKPAY_EVICTION_INTERVAL,
    BLINKPAY_ACTIVE_PROFILE,
    BLINKPAY_RETRY_ENABLED
}

/// <summary>
/// The extensions of BlinkPayProperty to retrieve the value based on the enum value as enum types cannot have constructors.
/// </summary>
public static class BlinkPayPropertyExtensions
{
    /// <summary>
    /// Returns the property name.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetPropertyName(this BlinkPayProperty property)
    {
        return property switch
        {
            BlinkPayProperty.BLINKPAY_DEBIT_URL => "blinkpay.debit.url",
            BlinkPayProperty.BLINKPAY_CLIENT_ID => "blinkpay.client.id",
            BlinkPayProperty.BLINKPAY_CLIENT_SECRET => "blinkpay.client.secret",
            BlinkPayProperty.BLINKPAY_MAX_CONNECTIONS => "blinkpay.max.connections",
            BlinkPayProperty.BLINKPAY_MAX_IDLE_TIME => "blinkpay.max.idle.time",
            BlinkPayProperty.BLINKPAY_MAX_LIFE_TIME => "blinkpay.max.life.time",
            BlinkPayProperty.BLINKPAY_PENDING_ACQUIRE_TIMEOUT => "blinkpay.pending.acquire.timeout",
            BlinkPayProperty.BLINKPAY_EVICTION_INTERVAL => "blinkpay.eviction.interval",
            BlinkPayProperty.BLINKPAY_ACTIVE_PROFILE => "blinkpay.active.profile",
            BlinkPayProperty.BLINKPAY_RETRY_ENABLED => "blinkpay.retry.enabled",
            _ => throw new ArgumentOutOfRangeException(nameof(property), property, "Unknown BlinkPayProperty")
        };
    }

    /// <summary>
    /// Returns the default value.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetDefaultValue(this BlinkPayProperty property) =>
        property switch
        {
            BlinkPayProperty.BLINKPAY_DEBIT_URL => null,
            BlinkPayProperty.BLINKPAY_CLIENT_ID => null,
            BlinkPayProperty.BLINKPAY_CLIENT_SECRET => null,
            BlinkPayProperty.BLINKPAY_MAX_CONNECTIONS => "10",
            BlinkPayProperty.BLINKPAY_MAX_IDLE_TIME => "PT20S",
            BlinkPayProperty.BLINKPAY_MAX_LIFE_TIME => "PT60S",
            BlinkPayProperty.BLINKPAY_PENDING_ACQUIRE_TIMEOUT => "PT10S",
            BlinkPayProperty.BLINKPAY_EVICTION_INTERVAL => "PT60S",
            BlinkPayProperty.BLINKPAY_ACTIVE_PROFILE => "test",
            BlinkPayProperty.BLINKPAY_RETRY_ENABLED => "true",
            _ => throw new ArgumentOutOfRangeException(nameof(property), property, "Unknown BlinkPayProperty")
        } ?? throw new InvalidOperationException();
}