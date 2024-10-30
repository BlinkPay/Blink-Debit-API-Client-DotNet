# Blink-Debit-API-Client-DotNet
[![CI](https://github.com/BlinkPay/Blink-Debit-API-Client-DotNet/actions/workflows/build.yml/badge.svg)](https://github.com/BlinkPay/Blink-Debit-API-Client-DotNet/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/BlinkDebitApiClient)](https://www.nuget.org/packages/BlinkDebitApiClient)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=blink-debit-api-client-dotnet&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=blink-debit-api-client-dotnet)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=blink-debit-api-client-dotnet&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=blink-debit-api-client-dotnet)
[![Snyk security](https://img.shields.io/badge/Snyk_security-monitored-9043C6)](https://app.snyk.io/org/blinkpay-zw9/project/d00cca34-5588-45f4-89a6-a23d568d0425)

# Table of Contents
1. [Introduction](#introduction)
2. [Contributing](#contributing)
3. [Minimum Requirements](#minimum-requirements)
4. [Dependency](#adding-the-dependency)
5. [Quick Start](#quick-start)
6. [Configuration](#configuration)
7. [Client Creation](#client-creation)
8. [Request ID, Correlation ID and Idempotency Key](#request-id-correlation-id-and-idempotency-key)
9. [Full Examples](#full-examples)
10. [Individual API Call Examples](#individual-api-call-examples)

## Introduction
This SDK allows merchants with .NET 8-based e-commerce sites to seamlessly integrate with Blink PayNow and Blink AutoPay in order to accept digital payments.

This SDK is written in C# 12.

## Contributing
We welcome contributions from the community. Your pull request will be reviewed by our team.

This project is licensed under the MIT License.

## Minimum Requirements
- .NET 8 or higher

## Adding the dependency
- If via your IDE, look for `BlinkDebitApiClient` in the NuGet tool
- If via .NET command line interface, run `dotnet add package BlinkDebitApiClient --version`
- If via `.csproj` file, add `<PackageReference Include="BlinkDebitApiClient" Version="<LATEST_VERSION>"/>`

## Quick Start
```csharp
var logger = LoggerFactory
    .Create(builder => builder
        .SetMinimumLevel(LogLevel.Information)
        .AddConsole()
        .AddDebug())
    .CreateLogger<MyProgram>();
var blinkpayUrl = "https://sandbox.debit.blinkpay.co.nz";
var clientId = "";
var clientSecret = "";
var timeout = 10000;
var client = new BlinkDebitClient(logger, blinkpayUrl, clientId, clientSecret, timeout);

var gatewayFlow = new GatewayFlow("https://www.blinkpay.co.nz/sample-merchant-return-page");
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr("particulars", "code", "reference");
var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

try {
    var qpCreateResponse = client.CreateQuickPayment(request);
    logger.LogInformation("Redirect URL: {}", qpCreateResponse.RedirectUri); // Redirect the consumer to this URL
    var qpId = qpCreateResponse.QuickPaymentId;
    var qpResponse = client.AwaitSuccessfulQuickPaymentOrThrowException(qpId, 300); // Will throw an exception if the payment was not successful after 5min
} catch (BlinkServiceException e) {
    logger.LogError("Encountered an error: " + e.Message);
}
```

## Configuration
- Customise/supply the required properties in your `appsettings.json` and/or `Properties/launchSettings.json`. This file should be available in your project folder.
- The BlinkPay **Sandbox** debit URL is `https://sandbox.debit.blinkpay.co.nz` and the **production** debit URL is `https://debit.blinkpay.co.nz`.
- The client credentials will be provided to you by BlinkPay as part of your on-boarding process.
- Properties can be supplied using environment variables.
> **Warning** Take care not to check in your client ID and secret to your source control.

### Configuration precedence
Configuration will be detected and loaded according to the hierarchy -
1. As provided directly to client constructor
2. Environment variables e.g. `export BLINKPAY_CLIENT_SECRET=...`
3. `Properties/launchSettings.json`
4. `appsettings.json`
5. Default values

### Configuration examples

#### Environment variables
The following values are recommended to be supplied using environment variables.
```shell
export BLINKPAY_DEBIT_URL=<BLINKPAY_DEBIT_URL>
export BLINKPAY_CLIENT_ID=<BLINKPAY_CLIENT_ID>
export BLINKPAY_CLIENT_SECRET=<BLINKPAY_CLIENT_SECRET>
```

### launchSettings file
If you want to use your launchSettings file locally, substitute the correct values to your `Properties/launchSettings.json` file. Do not commit this file into your repository.
```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "Demo": {
      "commandName": "Project",
      "environmentVariables": {
        "BLINKPAY_DEBIT_URL": "<BLINKPAY_DEBIT_URL>",
        "BLINKPAY_CLIENT_ID": "<BLINKPAY_CLIENT_ID>",
        "BLINKPAY_CLIENT_SECRET": "<BLINKPAY_CLIENT_SECRET>",
        "BLINKPAY_TIMEOUT": "10000",
        "BLINKPAY_RETRY_ENABLED": "true"
      }
    }
  }
}
```

#### appsettings file
To use your appsettings file, you can pass environment variables from command line or CI/CD for the placeholders into your `appsettings.json` file.
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    },
    "BlinkPay": {
        "DebitUrl": "{BLINKPAY_DEBIT_URL}",
        "ClientId": "{BLINKPAY_CLIENT_ID}",
        "ClientSecret": "{BLINKPAY_CLIENT_SECRET}",
        "Timeout": 10000,
        "RetryEnabled": true
    }
}
```

## Client creation
The client code can use .NET dependency injection.
```csharp
// configure dependency injection
var serviceCollection = new ServiceCollection();

// configure path to appsettings.json
var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
var config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json")
    .Build();
// configure BlinkPayProperties
serviceCollection.Configure<BlinkPayProperties>(config.GetSection("BlinkPay"));
serviceCollection.AddSingleton(resolver => resolver.GetRequiredService<IOptions<BlinkPayProperties>>().Value);

// configure logger
serviceCollection.AddLogging(builder =>
{
    builder
        .AddConsole() // use file logging
        .AddDebug(); // use information
});
var serviceProvider = serviceCollection.BuildServiceProvider();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("BlinkDebitClient");
serviceCollection.AddSingleton(logger);

// create BlinkDebitClient
serviceCollection.AddSingleton<BlinkDebitClient>();
serviceProvider = serviceCollection.BuildServiceProvider();

// retrieve BlinkDebitClient
var client = serviceProvider.GetService<BlinkDebitClient>();
```

Another way is to supply the required values during object creation:
```csharp
// configure logger
var logger = LoggerFactory
    .Create(builder => builder
        .AddConsole() // use file logging
        .AddDebug()) // use information
    .CreateLogger<MyProgram>();

// configure path to appsettings.json
var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
var config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json")
    .Build();
// bind BlinkPay settings section to BlinkPayProperties
var blinkPayProperties = new BlinkPayProperties();
config.GetSection("BlinkPay").Bind(blinkPayProperties);

// create BlinkDebitClient
var client = new BlinkDebitClient(logger, blinkPayProperties);

// or
// var client = new BlinkDebitClient(logger, blinkPayProperties.DebitUrl, blinkPayProperties.ClientId, blinkPayProperties.ClientSecret);
```

## Request ID, Correlation ID and Idempotency Key
An optional request ID, correlation ID and idempotency key can be added as arguments to API calls. They will be generated for you automatically if they are not provided.

A request can have one request ID and one idempotency key but multiple correlation IDs in case of retries.

## Full Examples
> **Note:** For error handling, a BlinkServiceException can be caught.
### Quick payment (one-off payment), using Gateway flow
A quick payment is a one-off payment that combines the API calls needed for both the consent and the payment.
```csharp
var gatewayFlow = new GatewayFlow("https://www.blinkpay.co.nz/sample-merchant-return-page");
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr("particulars", "code", "reference");
var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

var qpCreateResponse = client.CreateQuickPayment(request);
_logger.LogInformation("Redirect URL: {}", qpCreateResponse.RedirectUri); // Redirect the consumer to this URL
var qpId = qpCreateResponse.QuickPaymentId;
var qpResponse = client.AwaitSuccessfulQuickPaymentOrThrowException(qpId, 300); // Will throw an exception if the payment was not successful after 5min
```

### Single consent followed by one-off payment, using Gateway flow
```csharp
var redirectFlow = new RedirectFlow("https://www.blinkpay.co.nz/sample-merchant-return-page", Bank.BNZ);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr("particulars");
var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount);

var createConsentResponse = client.CreateSingleConsent(request);
var redirectUri = createConsentResponse.RedirectUri; // Redirect the consumer to this URL
var paymentRequest = new PaymentRequest
{
    ConsentId = createConsentResponse.ConsentId
};
var paymentResponse = client.CreatePayment(paymentRequest);
_logger.LogInformation("Payment Status: {}", client.GetPayment(paymentResponse.PaymentId).Status);
// TODO inspect the payment result status
```

## Individual API Call Examples
### Bank Metadata
Supplies the supported banks and supported flows on your account.
```csharp
var bankMetadataList = client.GetMeta();
```

### Quick Payments
#### Gateway Flow
```csharp
var gatewayFlow = new GatewayFlow(redirectUri);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Gateway Flow - Redirect Flow Hint
```csharp
var redirectFlowHint = new RedirectFlowHint(bank);
var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Gateway Flow - Decoupled Flow Hint
```csharp
var decoupledFlowHint = new DecoupledFlowHint(bank, identifierType, identifierValue);
var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Redirect Flow
```csharp
var redirectFlow = new RedirectFlow(redirectUri, bank);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Decoupled Flow
```csharp
var decoupledFlow = new DecoupledFlow(bank, identifierType, identifierValue, callbackUrl);
var authFlowDetail = new AuthFlowDetail(decoupledFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Retrieval
```csharp
var quickPaymentResponse = client.GetQuickPayment(quickPaymentId);
```
#### Revocation
```csharp
client.RevokeQuickPayment(quickPaymentId);
```

### Single/One-Off Consents
#### Gateway Flow
```csharp
var gatewayFlow = new GatewayFlow(redirectUri);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Gateway Flow - Redirect Flow Hint
```csharp
var redirectFlowHint = new RedirectFlowHint(bank);
var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Gateway Flow - Decoupled Flow Hint
```csharp
var decoupledFlowHint = new DecoupledFlowHint(bank, identifierType, identifierValue);
var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount);

CreateConsentResponse createConsentResponse = client.CreateSingleConsent(request);
```
#### Redirect Flow
Suitable for most consents.
```csharp
var redirectFlow = new RedirectFlow(redirectUri, bank);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Decoupled Flow
This flow type allows better support for mobile by allowing the supply of a mobile number or previous consent ID to identify the customer with their bank.

The customer will receive the consent request directly to their online banking app. This flow does not send the user through a web redirect flow.
```csharp
var decoupledFlow = new DecoupledFlow(bank, identifierType, identifierValue, callbackUrl);
var authFlowDetail = new AuthFlowDetail(decoupledFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Retrieval
Get the consent including its status
```csharp
var consent = client.GetSingleConsent(consentId);
```
#### Revocation
```csharp
client.RevokeSingleConsent(consentId);
```

### Blink AutoPay - Enduring/Recurring Consents
Request an ongoing authorisation from the customer to debit their account on a recurring basis.

Note that such an authorisation can be revoked by the customer in their mobile banking app.
#### Gateway Flow
```csharp
var gatewayFlow = new GatewayFlow(redirectUri);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Gateway Flow - Redirect Flow Hint
```csharp
var redirectFlowHint = new RedirectFlowHint(bank);
var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Gateway Flow - Decoupled Flow Hint
```csharp
var decoupledFlowHint = new DecoupledFlowHint(bank, identifierType, identifierValue);
var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Redirect Flow
```csharp
var redirectFlow = new RedirectFlow(redirectUri, bank;
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Decoupled Flow
```csharp
var decoupledFlow = new DecoupledFlow(bank, identifierType, identifierValue, callbackUrl);
var authFlowDetail = new AuthFlowDetail(decoupledFlow);
var authFlow = new AuthFlow(authFlowDetail);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount);
    
var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Retrieval
```csharp
var consent = client.GetEnduringConsent(consentId);
```
#### Revocation
```csharp
client.RevokeEnduringConsent(consentId);
```

### Payments
The completion of a payment requires a consent to be in the Authorised status.
#### Single/One-Off
```csharp
var paymentRequest = new PaymentRequest
{
    ConsentId = consentId
};

var paymentResponse = client.CreatePayment(request);
```
#### Enduring/Recurring
If you already have an approved consent, you can run a Payment against that consent at the frequency as authorised in the consent.
```csharp
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var enduringPaymentRequest = new EnduringPaymentRequest(pcr, amount);
var paymentRequest = new PaymentRequest(consentId, enduringPaymentRequest);

var paymentResponse = client.CreatePayment(request);
```
#### Retrieval
```csharp
var payment = client.GetPayment(paymentId);
```

### Refunds
#### Account Number Refund
```csharp
var request = new AccountNumberRefundRequest(paymentId);

var refundResponse = client.CreateRefund(request);
```
#### Full Refund (Not yet implemented)
```csharp
var pcr = new Pcr(particulars, code, reference);
var request = new FullRefundRequest(paymentId, pcr, redirectUri);

var refundResponse = client.CreateRefund(request);
```
#### Partial Refund (Not yet implemented)
```csharp
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var pcr = new Pcr(particulars, code, reference);
var request = new PartialRefundRequest(paymentId, amount pcr, redirectUri);

var refundResponse = client.CreateRefund(request);
```
#### Retrieval
```csharp
var refund = client.GetRefund(refundId);
```