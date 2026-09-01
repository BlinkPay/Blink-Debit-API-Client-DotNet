# CLAUDE.md - BlinkPay .NET SDK Code Knowledge

**Last Updated**: 2026-09-02
**Project**: Blink Debit API Client .NET SDK v1.5.0+
**Framework**: .NET 8.0 and .NET 10.0 (multi-targeted), C# 12

---

## Project Overview

This is a .NET SDK for integrating with the BlinkPay Debit API, supporting both one-off (PayNow) and recurring (AutoPay) payment flows. The SDK handles OAuth2 authentication, consent management, payment processing, and refunds.

**Repository**: BlinkPay/Blink-Debit-API-Client-DotNet
**API Version**: v1.0.54 (OpenAPI 3.0.4)
**Environments**: Sandbox (`https://sandbox.debit.blinkpay.co.nz`) and Production (`https://debit.blinkpay.co.nz`)

---

## API Design Philosophy

### Async-Only Methods

**All SDK methods are asynchronous** and return `Task<T>`. There are no synchronous wrapper methods.

```csharp
// ✅ Correct - Async method
var quickPayment = await client.CreateQuickPaymentAsync(request, headers);

// ❌ No synchronous alternative available
// var quickPayment = client.CreateQuickPayment(request, headers); // Does not exist
```

**Rationale**:
- **Performance**: I/O-bound operations (HTTP requests) benefit from async/await
- **Scalability**: Prevents thread pool exhaustion under load
- **Best Practices**: Aligns with modern .NET async patterns
- **Developer Experience**: Forces proper async usage, preventing common blocking pitfalls

**Implementation Notes**:
- All public API methods end with `Async` suffix
- Methods return `Task<T>` for operations returning data
- Methods return `Task` for operations without return values
- Callers must use `await` or properly handle the returned Task

---

## Key Implementation Details

### 1. OAuth Token Management

**Location**: `src/BlinkDebitApiClient/Client/Auth/OAuthAuthenticator.cs`

The SDK implements automatic token refresh with a 5-minute buffer before expiration:

```csharp
private bool IsTokenExpired(string token)
{
    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.Replace(BlinkDebitConstant.BEARER.GetValue(), string.Empty));
        // Add 5-minute buffer to refresh before actual expiration
        var expiryWithBuffer = jwtToken.ValidTo.AddMinutes(-5);
        return expiryWithBuffer <= DateTimeOffset.UtcNow;
    }
    catch
    {
        return true; // If token cannot be parsed, consider it expired
    }
}
```

**Implementation Notes**:
- Always add expiry buffer to prevent edge-case failures
- JWT token validation handles parse exceptions gracefully
- OAuth refresh logic is automatic and transparent
- **Thread-safe token refresh**: Uses `SemaphoreSlim` to prevent race conditions

#### Thread-Safe Token Management

The token refresh mechanism is **thread-safe** to prevent race conditions in high-concurrency scenarios:

```csharp
protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
{
    // Fast path: if token is valid, return immediately without acquiring semaphore
    if (!string.IsNullOrEmpty(Token) && !IsTokenExpired(Token))
    {
        return new HeaderParameter(KnownHeaders.Authorization, Token);
    }

    // Slow path: token needs refresh, acquire semaphore to ensure only one thread refreshes
    await _tokenRefreshSemaphore.WaitAsync().ConfigureAwait(false);
    try
    {
        // Double-check pattern: another thread might have refreshed while we were waiting
        if (string.IsNullOrEmpty(Token) || IsTokenExpired(Token))
        {
            Token = await GetToken().ConfigureAwait(false);
        }

        return new HeaderParameter(KnownHeaders.Authorization, Token);
    }
    finally
    {
        _tokenRefreshSemaphore.Release();
    }
}
```

**Thread-Safety Features**:
- **Fast path optimization**: Valid tokens bypass semaphore for maximum performance
- **SemaphoreSlim(1,1)**: Only one thread can refresh token at a time
- **Double-check locking**: Prevents duplicate OAuth requests if token was refreshed while waiting
- **Prevents race conditions**: Multiple concurrent requests won't trigger multiple token refreshes
- **Production-safe**: Eliminates authentication failures from corrupted token writes

**Why This Matters**:
- ❌ **Without thread safety**: 100 concurrent requests → 100 OAuth token requests (rate limiting, server load)
- ✅ **With thread safety**: 100 concurrent requests → 1 OAuth token request (all others wait and reuse)

---

### 2. Resource Management

**Locations**: `OAuthAuthenticator.cs`, `ApiClient.cs`

RestSharp RestClient instances must be properly disposed to prevent connection pool exhaustion:

```csharp
// Proper disposal pattern
using var client = new RestClient(clientOptions,
    configureSerialization: s => s.UseSerializer(() => new CustomJsonCodec(...)));
var response = await client.ExecuteAsync(request);
// Client automatically disposed
```

**Implementation Notes**:
- RestSharp RestClient implements IDisposable and must be disposed
- Use `using` statements for deterministic disposal
- Each API call creates a new RestClient (follows RestSharp best practices)
- Connection pooling handled by HttpClient internally
- RestSharp 111+ replaced `RestClientOptions.MaxTimeout` (int, ms) with `Timeout` (`TimeSpan?`) — the SDK's `Configuration.Timeout` remains an int in milliseconds, converted via `TimeSpan.FromMilliseconds()` in `ApiClient.cs`

---

### 3. Error Handling

**Location**: `src/BlinkDebitApiClient/Config/Configuration.cs:DefaultExceptionFactory`

The SDK implements robust null-safe error handling:

```csharp
// Null-safe error response handling
if (string.IsNullOrEmpty(response.RawContent))
{
    logger.LogError("Received HTTP {status} with empty response body", status);
    throw new BlinkServiceException($"HTTP {status}: Empty error response from server");
}

var body = JsonConvert.DeserializeObject<DetailErrorResponseModel>(response.RawContent);
if (body == null)
{
    logger.LogError("Could not parse error response for HTTP {status}. Content: {content}",
        status, response.RawContent);
    throw new BlinkServiceException($"HTTP {status}: Could not parse error response");
}

// Use TryGetValue for dictionary access
var correlationId = response.Headers.TryGetValue(
    BlinkDebitConstant.CORRELATION_ID.GetValue(), out var id)
    ? id.ToString()
    : "N/A";
```

**Implementation Notes**:
- Never trust external API responses to be well-formed
- Always validate before deserialization
- Use TryGetValue for dictionary access
- Log failures with context for debugging

---

### 4. Model Validation

**Location**: `src/BlinkDebitApiClient/Model/V1/Pcr.cs`

Optional fields require null-checking before validation:

```csharp
// Null-safe regex validation
private static readonly Regex RegexCode = new Regex(@"[a-zA-Z0-9- &#\?:_\/,\.']{0,12}",
    RegexOptions.Compiled | RegexOptions.CultureInvariant);

if (Code != null)
{
    if (false == RegexCode.Match(Code).Success)
    {
        yield return new ValidationResult("Invalid value for Code, must match a pattern of " + RegexCode,
            new[] { "Code" });
    }
}
```

**Implementation Notes**:
- Optional fields (Code, Reference) can be null
- Single consents: only Particulars required
- Enduring consents: all PCR fields (Particulars, Code, Reference) required
- Always null-check before regex validation on optional fields
- Regex patterns made static for performance

---

### 5. HTTP Status Code Mapping

**Location**: `Configuration.cs:DefaultExceptionFactory`

The SDK maps HTTP status codes to appropriate exception types:

```csharp
case 401:
case 403:
    throw new BlinkUnauthorisedException(body.Message);
case 404:
    throw new BlinkResourceNotFoundException(body.Message);
case 422:
    throw new BlinkInvalidValueException(body.Message);
case 502:
    throw new BlinkServiceException(body.Message);
default:
    throw new BlinkServiceException(body.Message);
```

**Implementation Notes**:
- HTTP 422 = validation error (invalid input)
- HTTP 401/403 = authentication/authorization error
- Exception type must match HTTP status semantics
- Affects error handling logic in consuming applications

---

### 6. Performance Optimizations

#### A. Regex Compilation

**Locations**: `Pcr.cs`, `Amount.cs`

```csharp
// Static compiled regex for better performance
private static readonly Regex RegexTotal = new Regex(@"^\d{1,13}\.\d{1,2}$",
    RegexOptions.Compiled | RegexOptions.CultureInvariant);
```

**Impact**: Significant performance improvement for repeated validations

#### B. Static Random for Retry Jitter

**Location**: `BlinkDebitClient.cs:ConfigureRetry()`

```csharp
private static readonly Random RetryRandom = new Random(); // Static field

RetryConfiguration.RetryPolicy = policyBuilder
    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        + TimeSpan.FromMilliseconds(RetryRandom.Next(0, 1000)));
```

#### C. Efficient Equality Checks

**Locations**: `EnduringConsentRequest.cs`, `SingleConsentRequest.cs`

```csharp
// Call base.Equals once, not multiple times
return base.Equals(input) &&
       (Flow == input.Flow || ...) &&
       (FromTimestamp == input.FromTimestamp || ...) &&
       ...
```

---

## Key Architecture Patterns

### 1. Authentication Flow

```
OAuthAuthenticator (implements IAuthenticator)
  ↓
GetAuthenticationParameter() - called before each request
  ↓
Checks: IsTokenExpired(Token)
  ↓
If expired: GetToken() - fetches new OAuth token
  ↓
Returns: HeaderParameter with Bearer token
```

**Key Points**:
- Token refresh is automatic and transparent
- 5-minute buffer before expiry
- JWT validation handles token parsing
- Each token request creates/disposes RestClient

### 2. API Client Pattern

```
BlinkDebitClient (facade)
  ↓
Uses: QuickPaymentsApi, SingleConsentsApi, EnduringConsentsApi, PaymentsApi, RefundsApi
  ↓
Each API uses: ApiClient (base HTTP client)
  ↓
ApiClient uses: OAuthAuthenticator
  ↓
Makes requests via: RestSharp RestClient (created per request)
```

**Key Points**:
- Facade pattern for simplified API access
- Each API operation creates new RestClient (using statement)
- Configuration shared across all APIs
- Logger injected at construction

### 3. Error Handling Strategy

```
API Response
  ↓
HTTP Status >= 400
  ↓
Configuration.ExceptionFactory
  ↓
Deserialize error body to DetailErrorResponseModel
  ↓
Map to specific exception:
  - 401/403 → BlinkUnauthorisedException
  - 404 → BlinkResourceNotFoundException
  - 422 → BlinkInvalidValueException
  - 502 → BlinkServiceException (with correlation ID)
  - Other → BlinkServiceException
```

**Key Points**:
- Centralized exception factory
- Correlation IDs for debugging 502 errors
- Null-safe deserialization
- Structured error logging

### 4. Retry Policy (Polly)

```
Exponential Backoff with Jitter
  ↓
Retry 1: 2s + random(0-1000ms)
Retry 2: 4s + random(0-1000ms)
Retry 3: 8s + random(0-1000ms)
```

**Conditions**: Configured via `RetryConfiguration.RetryEnabled`

---

## ASP.NET Core Integration

The SDK provides a dedicated NuGet package `BlinkDebitApiClient.Extensions.DependencyInjection` for seamless ASP.NET Core integration using Microsoft's dependency injection container.

### Package Structure

```
BlinkDebitApiClient.Extensions.DependencyInjection/
  ├── ServiceCollectionExtensions.cs   # Extension methods for IServiceCollection
  ├── BlinkDebitClientOptions.cs       # Configuration options class
  └── README.md                         # Package-specific documentation
```

### Registration Methods

**Two overloads** of `AddBlinkDebitClient()` are provided:

#### 1. Configuration Binding (Recommended for ASP.NET Core)

Binds from `IConfiguration` (typically `appsettings.json`):

```csharp
// Program.cs
using BlinkDebitApiClient.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlinkDebitClient(builder.Configuration);
// Uses "BlinkPay" section by default

// Or specify custom section name:
builder.Services.AddBlinkDebitClient(builder.Configuration, "CustomSection");
```

**appsettings.json**:
```json
{
  "BlinkPay": {
    "DebitUrl": "https://sandbox.debit.blinkpay.co.nz",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "TimeoutSeconds": 10,
    "RetryEnabled": true
  }
}
```

#### 2. Programmatic Configuration

Configure via Action delegate:

```csharp
builder.Services.AddBlinkDebitClient(options =>
{
    options.DebitUrl = "https://sandbox.debit.blinkpay.co.nz";
    options.ClientId = builder.Configuration["BlinkPay:ClientId"];
    options.ClientSecret = builder.Configuration["BlinkPay:ClientSecret"];
    options.TimeoutSeconds = 15;
    options.RetryEnabled = true;
});
```

### BlinkDebitClientOptions

**Properties**:
- `DebitUrl` (required): Base URL for Blink Debit API
- `ClientId` (required): OAuth2 client ID
- `ClientSecret` (required): OAuth2 client secret
- `TimeoutSeconds` (optional, default: 10): Request timeout in seconds
- `RetryEnabled` (optional, default: true): Enable/disable retry policy

**Validation**: Options are validated on service resolution (fail-fast). Missing required fields throw `InvalidOperationException` with helpful error messages.

### Interface-Based Injection

The DI extension registers `IBlinkDebitClient` as a singleton:

```csharp
public class PaymentController : ControllerBase
{
    private readonly IBlinkDebitClient _blinkClient;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IBlinkDebitClient blinkClient, ILogger<PaymentController> logger)
    {
        _blinkClient = blinkClient;
        _logger = logger;
    }

    [HttpPost("quick-payment")]
    public async Task<IActionResult> CreateQuickPayment([FromBody] QuickPaymentDto dto)
    {
        var gatewayFlow = new GatewayFlow(dto.RedirectUri);
        var authFlowDetail = new AuthFlowDetail(gatewayFlow);
        var authFlow = new AuthFlow(authFlowDetail);
        var pcr = new Pcr(dto.Particulars, dto.Code, dto.Reference);
        var amount = new Amount(dto.Amount, Amount.CurrencyEnum.NZD);
        var request = new QuickPaymentRequest(authFlow, pcr, amount);

        try
        {
            var response = await _blinkClient.CreateQuickPaymentAsync(request);
            return Ok(new { redirectUri = response.RedirectUri, quickPaymentId = response.QuickPaymentId });
        }
        catch (BlinkServiceException ex)
        {
            _logger.LogError(ex, "Failed to create quick payment");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
```

### Key Implementation Details

**Location**: `src/BlinkDebitApiClient.Extensions.DependencyInjection/ServiceCollectionExtensions.cs`

```csharp
services.AddSingleton<IBlinkDebitClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<BlinkDebitClientOptions>>().Value;
    var logger = sp.GetRequiredService<ILogger<BlinkDebitClient>>();

    // Validate options (throws InvalidOperationException if invalid)
    options.Validate();

    // Create BlinkDebitClient with validated options
    return new BlinkDebitClient(
        logger,
        options.DebitUrl + "/payments/v1",
        options.ClientId,
        options.ClientSecret
    );
});
```

**Key Points**:
- **Singleton lifetime**: Follows HTTP client best practices (connection pooling)
- **Logger injection**: Uses ASP.NET Core's logging infrastructure
- **Options pattern**: Integrates with `IOptions<T>` for configuration
- **Validation**: Required fields validated on service resolution (fail-fast)
- **URL construction**: `/payments/v1` appended automatically to `DebitUrl`

### Security Best Practices

**Never hardcode credentials in appsettings.json**. Use one of these approaches:

#### 1. User Secrets (Development)
```bash
dotnet user-secrets init
dotnet user-secrets set "BlinkPay:ClientId" "your-client-id"
dotnet user-secrets set "BlinkPay:ClientSecret" "your-client-secret"
```

#### 2. Environment Variables (Production)
```bash
export BlinkPay__ClientId="your-client-id"
export BlinkPay__ClientSecret="your-client-secret"
# Note: __ (double underscore) for nested config
```

#### 3. Azure Key Vault / AWS Secrets Manager
```csharp
builder.Configuration.AddAzureKeyVault(...);
// Secrets injected into configuration
```

### Testing with DI

**Location**: `src/BlinkDebitApiClient.Extensions.DependencyInjection.Test/ServiceCollectionExtensionsTests.cs`

```csharp
[Fact]
public void AddBlinkDebitClient_WithConfiguration_RegistersClient()
{
    // Arrange
    var configDictionary = new Dictionary<string, string?>
    {
        ["BlinkPay:DebitUrl"] = "https://sandbox.debit.blinkpay.co.nz",
        ["BlinkPay:ClientId"] = "test-client-id",
        ["BlinkPay:ClientSecret"] = "test-client-secret"
    };

    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configDictionary)
        .Build();

    var services = new ServiceCollection();
    services.AddLogging(builder => builder.AddConsole());

    // Act
    services.AddBlinkDebitClient(configuration);
    var serviceProvider = services.BuildServiceProvider();

    // Assert
    var client = serviceProvider.GetService<IBlinkDebitClient>();
    Assert.NotNull(client);
    Assert.IsAssignableFrom<BlinkDebitClient>(client);
}
```

**Test Coverage**:
- ✅ Registration with Action configuration
- ✅ Registration with IConfiguration binding
- ✅ Custom section name support
- ✅ Singleton lifetime verification
- ✅ Null argument validation
- ✅ Missing required options validation (DebitUrl, ClientId, ClientSecret)

### Migration Guide

**From manual DI** → **To extension package**:

**Before**:
```csharp
services.Configure<BlinkPayProperties>(config.GetSection("BlinkPay"));
services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<BlinkPayProperties>>().Value);
services.AddLogging(builder => builder.AddConsole().AddDebug());
var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("BlinkDebitClient");
services.AddSingleton(logger);
services.AddSingleton<BlinkDebitClient>();
```

**After**:
```csharp
services.AddBlinkDebitClient(builder.Configuration);
```

**Changes required**:
1. Install `BlinkDebitApiClient.Extensions.DependencyInjection` package
2. Replace manual registration with `AddBlinkDebitClient()`
3. Change dependency from `BlinkDebitClient` to `IBlinkDebitClient`
4. Update appsettings structure if needed (see BlinkDebitClientOptions)

---

## Testing Patterns

### Test Structure

```csharp
[Collection("Blink Debit Collection")] // Shared fixture
public class SomeApiTests : IDisposable
{
    private readonly BlinkDebitClient _client;
    private static readonly Dictionary<string, string?> RequestHeaders;

    public SomeApiTests(BlinkDebitFixture fixture)
    {
        _client = fixture.BlinkDebitClient;
        // Setup request headers (request ID, correlation ID, etc.)
    }

    [Fact(DisplayName = "Human-readable test description")]
    public async Task TestMethod() // Always async Task, never async void (xUnit1048)
    {
        // Arrange
        var request = new SomeRequest(...);

        // Act
        var response = await _client.SomeOperation(request, RequestHeaders);

        // Assert
        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
    }

    public void Dispose()
    {
        RequestHeaders.Clear();
    }
}
```

### Test Fixture Pattern

```csharp
public class BlinkDebitFixture
{
    public BlinkDebitFixture()
    {
        // Read credentials from environment variables
        var initialConfiguration = new Configuration
        {
            BasePath = "https://sandbox.debit.blinkpay.co.nz/payments/v1",
            OAuthTokenUrl = "https://sandbox.debit.blinkpay.co.nz/oauth2/token",
            OAuthClientId = Environment.GetEnvironmentVariable("BLINKPAY_CLIENT_ID") ?? string.Empty,
            OAuthClientSecret = Environment.GetEnvironmentVariable("BLINKPAY_CLIENT_SECRET") ?? string.Empty,
            OAuthFlow = OAuthFlow.APPLICATION
        };

        // Create API instances
        BlinkDebitClient = new BlinkDebitClient(logger, apiClient, finalConfiguration);
    }
}

[CollectionDefinition("Blink Debit Collection")]
public class BlinkDebitCollection : ICollectionFixture<BlinkDebitFixture>
{
}
```

**Key Points**:
- Fixture pattern for shared setup
- Environment variables for credentials (never hardcode!)
- Collection fixture for efficient test execution
- IDisposable for cleanup

### Integration Test Guidelines

1. **Environment Variables Required**:
   ```bash
   export BLINKPAY_CLIENT_ID="your-client-id"
   export BLINKPAY_CLIENT_SECRET="your-client-secret"
   ```

2. **Sandbox URLs**:
   - API: `https://sandbox.debit.blinkpay.co.nz/payments/v1`
   - OAuth: `https://sandbox.debit.blinkpay.co.nz/oauth2/token`

3. **Test Categories**:
   - Unit tests: Model validation, helpers
   - Integration tests: Actual API calls to sandbox
   - Some tests marked `Skip` if requiring user authorization

4. **Request Headers** (always include):
   - `X-Request-Id`: Unique per request
   - `X-Correlation-Id`: Track related requests
   - `X-Customer-IP`: End user IP
   - `X-Customer-User-Agent`: End user agent
   - `Idempotency-Key`: Prevent duplicate operations

---

## Common Patterns & Usage

### 1. PCR (Particulars, Code, Reference) Fields

**Different requirements for single vs enduring consents:**

```csharp
// Single Consent - only Particulars required
var pcr = new Pcr("particulars"); // Code and Reference optional

// Enduring Consent - all fields required
var pcr = new Pcr("particulars", "code", "reference"); // All required
```

### 2. Amount Validation

**Pattern**: Must match `^\d{1,13}\.\d{1,2}$`

**Valid**: `"1.25"`, `"0.01"`, `"999999999999.99"`
**Invalid**: `"1.5"` (missing decimal), `"1"` (no decimals), `1.25` (not string)

### 3. Flow Types

**Gateway Flow**: BlinkPay selects bank or shows bank picker
```csharp
var gatewayFlow = new GatewayFlow(redirectUri);
// Optional: Add flow hint (redirect or decoupled)
var flowHint = new GatewayFlowAllOfFlowHint(new RedirectFlowHint(Bank.PNZ));
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
```

**Redirect Flow**: Direct to specific bank
```csharp
var redirectFlow = new RedirectFlow(redirectUri, Bank.PNZ);
```

**Decoupled Flow**: Mobile-first, no redirect
```csharp
var decoupledFlow = new DecoupledFlow(Bank.PNZ, IdentifierType.PhoneNumber, "+64-123456789", callbackUrl);
```

**PNZ Auto-Approve (Sandbox Testing)**:
```csharp
// Use special phone number for automated testing without manual authorization
var decoupledFlow = new DecoupledFlow(
    Bank.PNZ,                    // Only PNZ supports auto-approve
    IdentifierType.PhoneNumber,
    "+64-259531933",             // Magic phone number - triggers auto-approval
    callbackUrl
);
```

**Key Differences**:
- **RedirectUri**: DecoupledFlow returns `null` for RedirectUri (no browser redirect needed)
- **Gateway/RedirectFlow**: Returns a non-null RedirectUri for browser-based flows
- **Status Flow**: DecoupledFlow → `AwaitingAuthorisation` → `Authorised` (skips `GatewayAwaitingSubmission`)

**Auto-Approve Implementation**:
- Phone number `+64-259531933` automatically approves consents in sandbox
- Enables fully automated integration testing
- Only works with `Bank.PNZ` in sandbox environment
- Production flows require real user authorization

### 4. Consent Status Transitions

```
GatewayAwaitingSubmission → AwaitingAuthorisation → Authorised → Consumed
                          ↘                        ↘          ↘
                            Revoked                Rejected    Rejected
```

**Key**: Wait for `Authorised` before creating payment.

### 5. Idempotency

**Idempotency keys are auto-generated** for all POST operations (consents, payments, refunds). You can optionally provide your own:
```csharp
// Optional - SDK auto-generates if not provided
RequestHeaders[BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = "my-custom-key";
```

**Key behavior**: Same key = same result (idempotent retry, not duplicate). Keys are automatically reused across retry attempts.

### 6. Correlation IDs for Support

**HTTP 502 errors include correlation IDs**:
```csharp
var correlationId = response.Headers.TryGetValue(
    BlinkDebitConstant.CORRELATION_ID.GetValue(), out var id) ? id.ToString() : "N/A";
```

**Always log correlation IDs** - required for BlinkPay support.

---

## Best Practices

### 1. Resource Management
```csharp
// ✓ GOOD
using var client = new RestClient(...);

// ✓ GOOD
using var stream = File.OpenRead(path);

// ✗ AVOID
var client = new RestClient(...);
// Missing disposal
```

### 2. Null Safety
```csharp
// ✓ GOOD
if (optionalField != null)
{
    // Validate
}

// ✓ GOOD
dict.TryGetValue(key, out var value);

// ✗ AVOID
var value = dict[key]; // Throws if missing
```

### 3. Logging
```csharp
// ✓ GOOD - structured logging
_logger.LogError("HTTP {status} with content: {content}", status, response.RawContent);

// ✗ AVOID - string concatenation
_logger.LogError("HTTP " + status + " with content: " + response.RawContent);
```

### 4. Configuration
```csharp
// Precedence order (highest to lowest):
// 1. Constructor parameters
// 2. Environment variables (BLINKPAY_CLIENT_ID, etc.)
// 3. Properties/launchSettings.json
// 4. appsettings.json
// 5. Default values
```

### 5. Error Handling
```csharp
// ✓ GOOD
try
{
    var payment = client.CreateQuickPayment(request);
}
catch (BlinkInvalidValueException e)
{
    // Handle validation errors
}
catch (BlinkUnauthorisedException e)
{
    // Handle auth errors
}
catch (BlinkServiceException e)
{
    // Handle other API errors
}
```

---

## Idempotency-Key Handling

For POST operations (create consent, payment, refund), the SDK **automatically generates a UUID** for the `idempotency-key` header if not provided by the caller.

### Auto-generation

The following operations automatically generate idempotency keys:
- `CreateSingleConsentAsync` / `CreateSingleConsent`
- `CreateEnduringConsentAsync` / `CreateEnduringConsent`
- `CreateQuickPaymentAsync` / `CreateQuickPayment`
- `CreatePaymentAsync` / `CreatePayment`
- `CreateRefundAsync` / `CreateRefund`

### Key Reuse Across Retries

The same idempotency-key is **automatically reused across all retry attempts** by Polly's retry policy, ensuring idempotent behavior even during transient failures.

**Implementation**: When a retry occurs, the same `RequestOptions` object (with the same headers) is reused, preserving the idempotency-key generated on the first attempt.

### Custom Idempotency Key

Callers can still provide their own idempotency-key via `requestHeaders`:

```csharp
var headers = new Dictionary<string, string?>
{
    [BlinkDebitConstant.IDEMPOTENCY_KEY.GetValue()] = "my-custom-key-12345"
};
await client.CreateSingleConsentAsync(headers, consentRequest);
```

### Why This Matters

**Without idempotency-key**:
- Network errors during retries could create duplicate consents/payments
- Multiple identical requests could process multiple times

**With auto-generated idempotency-key**:
- ✅ Same key reused across retries = server recognizes duplicate request
- ✅ Server returns the original result instead of creating duplicates
- ✅ Safe to retry without manual key management

**Best Practice**: Let the SDK auto-generate the key unless you have a specific business requirement for custom keys (e.g., tying operations to external transaction IDs).

---

## Request ID and Correlation ID Handling

The SDK **automatically generates UUIDs** for both `request-id` and `x-correlation-id` headers if not provided by the caller. This ensures consistent distributed tracing across all API operations.

### Auto-generation

**All API operations** automatically generate these headers:
- `request-id`: Uniquely identifies a single API request
- `x-correlation-id`: Groups related requests together (e.g., create consent → create payment)

### Header Reuse Across Retries

Both `request-id` and `x-correlation-id` are **automatically reused across all retry attempts** by Polly's retry policy, ensuring:
- ✅ The same request-id is preserved across retries
- ✅ The same correlation-id links all retry attempts
- ✅ Complete traceability in logs and monitoring systems

**Implementation**: When a retry occurs, the same `RequestOptions` object (with the same headers) is reused, preserving both IDs generated on the first attempt.

### Custom Request/Correlation IDs

Callers can provide their own IDs via `requestHeaders`:

```csharp
var headers = new Dictionary<string, string?>
{
    [BlinkDebitConstant.REQUEST_ID.GetValue()] = "my-request-id-12345",
    [BlinkDebitConstant.CORRELATION_ID.GetValue()] = "my-correlation-id-67890"
};
await client.CreateSingleConsentAsync(consentRequest, headers);
```

### Why This Matters

**For Distributed Tracing**:
- ✅ Every request has a unique request-id for debugging
- ✅ Related operations share the same correlation-id
- ✅ Retries preserve IDs, showing they're the same logical operation
- ✅ Logs include correlation-id in scope for easy filtering

**For Support**:
- ✅ Correlation IDs are included in HTTP 502 error responses
- ✅ Provide correlation-id to BlinkPay support for troubleshooting
- ✅ Request IDs help identify specific failed requests in logs

**Best Practice**:
- Let the SDK auto-generate IDs for most use cases
- Provide custom correlation-id when linking operations across multiple API calls (e.g., create consent in one call, create payment in another)
- Always log correlation IDs for support requests

---

## Performance Considerations

1. **Static Regex**: Always use `RegexOptions.Compiled` for reused patterns
2. **RestClient per request**: Not a performance issue (HttpClient pooling underneath)
3. **Token refresh**: Cached with 5-minute buffer, minimal overhead
4. **Retry policy**: 3 attempts max, exponential backoff prevents thundering herd
5. **Async/await**: Use async methods (`*Async`) for all I/O operations
6. **Auto-generated headers**: Request-ID, correlation-ID, and idempotency-key auto-generated as UUIDs with minimal overhead
7. **Header reuse**: All headers preserved across retries for consistent tracing

---

## Security Checklist

- ✓ Never hardcode credentials
- ✓ Use environment variables or secure vaults
- ✓ HTTPS only (enforced by SDK)
- ✓ JWT tokens validated before use
- ✓ Correlation IDs for audit trails
- ✓ Request IDs for tracing
- ✓ Idempotency keys for duplicate prevention
- ✓ No credentials in logs (redacted)
- ✓ No credentials in test files (use Environment.GetEnvironmentVariable)

---

## Useful Commands

### Build & Test
```bash
dotnet restore
dotnet build --configuration Release
dotnet test --logger "console;verbosity=detailed"
```

### Run Specific Tests
```bash
# README integration tests
dotnet test --filter "FullyQualifiedName~ReadmeExamplesIntegrationTests"

# Specific API tests
dotnet test --filter "FullyQualifiedName~QuickPaymentsApiTests"
```

### Set Test Credentials
```bash
export BLINKPAY_CLIENT_ID="your-client-id"
export BLINKPAY_CLIENT_SECRET="your-client-secret"
```

---

## Key Files Reference

### Authentication/Security
- `src/BlinkDebitApiClient/Client/Auth/OAuthAuthenticator.cs`
- `src/BlinkDebitApiClient/Config/Configuration.cs`

### API Clients
- `src/BlinkDebitApiClient/Client/ApiClient.cs`
- `src/BlinkDebitApiClient/Api/V1/BlinkDebitClient.cs`
- `src/BlinkDebitApiClient/Api/V1/IBlinkDebitClient.cs`

### Dependency Injection Extensions
- `src/BlinkDebitApiClient.Extensions.DependencyInjection/ServiceCollectionExtensions.cs`
- `src/BlinkDebitApiClient.Extensions.DependencyInjection/BlinkDebitClientOptions.cs`

### Data Models (OpenAPI Generated)
- `src/BlinkDebitApiClient/Model/V1/*.cs`

### Tests
- `src/BlinkDebitApiClient.Test/Api/V1/*Tests.cs`
- `src/BlinkDebitApiClient.Extensions.DependencyInjection.Test/ServiceCollectionExtensionsTests.cs`

---

## Development Checklist

When making changes, consider:

1. **Authentication**: Does this affect token refresh logic?
2. **Resource Management**: Are all IDisposable objects disposed?
3. **Null Safety**: Can this field be null? Is there validation?
4. **OpenAPI Compliance**: Does the model match the spec?
5. **Error Handling**: What exceptions can be thrown? Are they handled?
6. **Performance**: Is this pattern efficient? (static regex, etc.)
7. **Testing**: Are there integration tests covering this?
8. **Security**: Are credentials handled safely?
9. **Logging**: Is there enough context for debugging?
10. **Documentation**: Is the README updated?

---

**End of CLAUDE.md**
