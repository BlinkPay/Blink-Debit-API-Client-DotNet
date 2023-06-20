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
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BlinkDebitApiClient.Config;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace BlinkDebitApiClient.Client.Auth;

/// <summary>
/// An authenticator for OAuth2 authentication flows
/// </summary>
public class OAuthAuthenticator : AuthenticatorBase
{
    private readonly string _tokenUrl;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _grantType;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly IReadableConfiguration _configuration;
    private static readonly TimeZoneInfo NzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

    /// <summary>
    /// Initialize the OAuth2 Authenticator
    /// </summary>
    public OAuthAuthenticator(
        string tokenUrl,
        string clientId,
        string clientSecret,
        OAuthFlow? flow,
        JsonSerializerSettings serializerSettings,
        IReadableConfiguration configuration) : base("")
    {
        _tokenUrl = tokenUrl;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _serializerSettings = serializerSettings;
        _configuration = configuration;

        if (flow == OAuthFlow.APPLICATION) _grantType = "client_credentials";
    }

    /// <summary>
    /// Creates an authentication parameter from an access token.
    /// </summary>
    /// <param name="accessToken">Access token to create a parameter from.</param>
    /// <returns>An authentication parameter.</returns>
    protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        Token = string.IsNullOrEmpty(Token) ? await GetToken().ConfigureAwait(false) : Token;
        
        // check if access token has expired
        if (!string.IsNullOrEmpty(Token))
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(Token.Replace("Bearer ", string.Empty));
            var expiry = TimeZoneInfo.ConvertTime(token.ValidTo, NzTimeZone);
            var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, NzTimeZone);
            if (expiry > now)
            {
                Token = string.IsNullOrEmpty(Token) ? await GetToken().ConfigureAwait(false) : Token;
            }
        }

        return new HeaderParameter(KnownHeaders.Authorization, Token);
    }

    /// <summary>
    /// Gets the token from the OAuth2 server.
    /// </summary>
    /// <returns>An authentication token.</returns>
    private async Task<string> GetToken()
    {
        var client = new RestClient(_tokenUrl,
            configureSerialization: s => s.UseSerializer(() => new CustomJsonCodec(_serializerSettings, _configuration)));

        var request = new RestRequest()
            .AddParameter("grant_type", _grantType)
            .AddParameter("client_id", _clientId)
            .AddParameter("client_secret", _clientSecret);
        var response = await client.PostAsync<TokenResponse>(request).ConfigureAwait(false);
        return $"{response.TokenType} {response.AccessToken}";
    }
}