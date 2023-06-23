# Blink-Debit-API-Client-CSharp-DotNet
[![CI](https://github.com/BlinkPay/Blink-Debit-API-Client-CSharp-DotNet/actions/workflows/maven-build.yml/badge.svg)](https://github.com/BlinkPay/Blink-Debit-API-Client-CSharp-DotNet/actions/workflows/maven-build.yml)

---

[![blink-debit-api-client-csharp-dotnet](https://buildstats.info/nuget/BlinkDebitApiClient)](https://www.nuget.org/packages/BlinkDebitApiClient)
[![Sonar](https://sonarcloud.io/api/project_badges/measure?project=blink-debit-api-client-csharp-dotnet&metric=alert_status)](https://sonarcloud.io/dashboard?id=blink-debit-api-client-csharp-dotnet)
[![Snyk](https://snyk-widget.herokuapp.com/badge/mvn/nz.co.blinkpay/blink-debit-api-client-csharp-dotnet/latest/badge.svg)](https://security.snyk.io/package/maven/nz.co.blinkpay:blink-debit-api-client-csharp-dotnet/latest)

# Table of Contents
1. [Minimum Requirements](#minimum-requirements)
2. [Dependency](#adding-the-dependency)
3. [Quick Start](#quick-start)
4. [Configuration](#configuration)
5. [Client Creation](#client-creation)
6. [Request ID, Correlation ID and Idempotency Key](#request-id-correlation-id-and-idempotency-key)
7. [Full Examples](#full-examples)
8. [Individual API Call Examples](#individual-api-call-examples)

This SDK allows merchants with .NET 7-based e-commerce site to integrate with Blink PayNow and Blink AutoPay.

This SDK was written in C# 11.

## Minimum Requirements
- .NET 7 or higher

## Adding the dependency
- If via your IDE, look for `BlinkDebitApiClient` in the NuGet tool
- If via .NET command line interface, run `dotnet add package BlinkDebitApiClient --version`
- If via `.csproj` file, add `<PackageReference Include="BlinkDebitApiClient" Version="1.0.1"/>`

## Quick Start
```csharp
var logger = LoggerFactory
    .Create(builder => builder
        .AddConsole()
        .AddDebug())
    .CreateLogger<MyProgram>();
var blinkpayUrl = "https://sandbox.debit.blinkpay.co.nz";
var clientId = "...";
var clientSecret = "...";
var client = new BlinkDebitClient(logger, blinkpayUrl, clientId, clientSecret);

var gatewayFlow = new GatewayFlow("https://www.blinkpay.co.nz/sample-merchant-return-page", null, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr("particulars", "code", "reference");
var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var qpCreateResponse = client.CreateQuickPayment(request);
_logger.LogInformation("Redirect URL: {}", qpCreateResponse.RedirectUri); // Redirect the consumer to this URL
var qpId = qpCreateResponse.QuickPaymentId;
var qpResponse = client.AwaitSuccessfulQuickPaymentOrThrowException(qpId, 300); // Will throw an exception if the payment was not successful after 5min
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
```shell
export BLINKPAY_DEBIT_URL=<BLINKPAY_DEBIT_URL>
export BLINKPAY_CLIENT_ID=<BLINKPAY_CLIENT_ID>
export BLINKPAY_CLIENT_SECRET=<BLINKPAY_CLIENT_SECRET>
export BLINKPAY_TIMEOUT=00:00:10
export BLINKPAY_RETRY_ENABLED=true
```

### launchSettings file
Substitute the correct values to your `Properties/launchSettings.json` file. Make sure this file is NOT pushed to the repository.

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
        "BLINKPAY_TIMEOUT": "00:00:10",
        "BLINKPAY_RETRY_ENABLED": "true"
      }
    }
  }
}
```

#### appsettings file
Substitute the correct values to your `appsettings.json` file. The `ClientId` and `ClientSecret` have been omitted so as NOT to accidentally push it to the repository.
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
        "DebitUrl": "<BLINK_DEBIT_URL>",
        "Timeout": "00:00:10",
        "RetryEnabled": "true"
    }
}
```

## Client creation
The client code can use .NET dependency injection.
```csharp
// configure dependency injection
var serviceCollection = new ServiceCollection();

// configure logger
serviceCollection.AddLogging(builder =>
{
    builder
        .AddConsole() // use file logging
        .AddDebug(); // use information
});

// configure path to appsettings.json
var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
var config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json")
    .Build();
// configure BlinkPayProperties
serviceCollection.Configure<BlinkPayProperties>(config.GetSection("BlinkPay"));

// create BlinkDebitClient
serviceCollection.AddSingleton<BlinkDebitClient>(serviceProvider =>
{
    return new BlinkDebitClient(serviceProvider.GetRequiredService<ILogger<RefundsApi>>(),
        serviceProvider.GetRequiredService<IOptions<BlinkPayProperties>>().Value);
});

// build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

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
### Quick payment (one-off payment), using Gateway flow
A quick payment is a one-off payment that combines the API calls needed for both the consent and the payment.
```csharp
var gatewayFlow = new GatewayFlow("https://www.blinkpay.co.nz/sample-merchant-return-page", null, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr("particulars", "code", "reference");
var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var qpCreateResponse = client.CreateQuickPayment(request);
_logger.LogInformation("Redirect URL: {}", qpCreateResponse.RedirectUri); // Redirect the consumer to this URL
var qpId = qpCreateResponse.QuickPaymentId;
var qpResponse = client.AwaitSuccessfulQuickPaymentOrThrowException(qpId, 300); // Will throw an exception if the payment was not successful after 5min
```

### Single consent followed by one-off payment, using Gateway flow
```csharp
var redirectFlow = new RedirectFlow("https://www.blinkpay.co.nz/sample-merchant-return-page", Bank.BNZ, AuthFlowDetail.TypeEnum.Redirect);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr("particulars");
var amount = new Amount("0.01", Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createConsentResponse = client.CreateSingleConsent(consent);
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
var gatewayFlow = new GatewayFlow(redirectUri, null, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Gateway Flow - Redirect Flow Hint
```csharp
var redirectFlowHint = new RedirectFlowHint(bank, FlowHint.TypeEnum.Redirect);
var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Gateway Flow - Decoupled Flow Hint
```csharp
var decoupledFlowHint = new DecoupledFlowHint(bank, identifierType, identifierValue,
    FlowHint.TypeEnum.Decoupled);
var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Redirect Flow
```csharp
var redirectFlow = new RedirectFlow(redirectUri, bank, AuthFlowDetail.TypeEnum.Redirect);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createQuickPaymentResponse = client.CreateQuickPayment(request);
```
#### Decoupled Flow
```csharp
var decoupledFlow = new DecoupledFlow(bank, identifierType, identifierValue,
    callbackUrl, AuthFlowDetail.TypeEnum.Decoupled);
var authFlowDetail = new AuthFlowDetail(decoupledFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new QuickPaymentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

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
var gatewayFlow = new GatewayFlow(redirectUri, null, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Gateway Flow - Redirect Flow Hint
```csharp
var redirectFlowHint = new RedirectFlowHint(bank, FlowHint.TypeEnum.Redirect);
var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Gateway Flow - Decoupled Flow Hint
```csharp
var decoupledFlowHint = new DecoupledFlowHint(bank, identifierType, identifierValue,
    FlowHint.TypeEnum.Decoupled);
var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

CreateConsentResponse createConsentResponse = client.CreateSingleConsent(request);
```
#### Redirect Flow
Suitable for most consents.
```csharp
var redirectFlow = new RedirectFlow(redirectUri, bank, AuthFlowDetail.TypeEnum.Redirect);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

var createConsentResponse = client.CreateSingleConsent(request);
```
#### Decoupled Flow
This flow type allows better support for mobile by allowing the supply of a mobile number or previous consent ID to identify the customer with their bank.

The customer will receive the consent request directly to their online banking app. This flow does not send the user through a web redirect flow.
```csharp
var decoupledFlow = new DecoupledFlow(bank, identifierType, identifierValue,
    callbackUrl, AuthFlowDetail.TypeEnum.Decoupled);
var authFlowDetail = new AuthFlowDetail(decoupledFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new SingleConsentRequest(authFlow, pcr, amount, ConsentDetail.TypeEnum.Single);

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
var gatewayFlow = new GatewayFlow(redirectUri, null, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount,
    ConsentDetail.TypeEnum.Enduring);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Gateway Flow - Redirect Flow Hint
```csharp
var redirectFlowHint = new RedirectFlowHint(bank, FlowHint.TypeEnum.Redirect);
var flowHint = new GatewayFlowAllOfFlowHint(redirectFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount,
    ConsentDetail.TypeEnum.Enduring);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Gateway Flow - Decoupled Flow Hint
```csharp
var decoupledFlowHint = new DecoupledFlowHint(bank, identifierType, identifierValue,
    FlowHint.TypeEnum.Decoupled);
var flowHint = new GatewayFlowAllOfFlowHint(decoupledFlowHint);
var gatewayFlow = new GatewayFlow(redirectUri, flowHint, AuthFlowDetail.TypeEnum.Gateway);
var authFlowDetail = new AuthFlowDetail(gatewayFlow);
var authFlow = new AuthFlow(authFlowDetail);
var pcr = new Pcr(particulars, code, reference);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount,
    ConsentDetail.TypeEnum.Enduring);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Redirect Flow
```csharp
var redirectFlow = new RedirectFlow(redirectUri, bank, AuthFlowDetail.TypeEnum.Redirect);
var authFlowDetail = new AuthFlowDetail(redirectFlow);
var authFlow = new AuthFlow(authFlowDetail);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount,
    ConsentDetail.TypeEnum.Enduring);

var createConsentResponse = client.CreateEnduringConsent(request);
```
#### Decoupled Flow
```csharp
var decoupledFlow = new DecoupledFlow(bank, identifierType, identifierValue,
    callbackUrl, AuthFlowDetail.TypeEnum.Decoupled);
var authFlowDetail = new AuthFlowDetail(decoupledFlow);
var authFlow = new AuthFlow(authFlowDetail);
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var request = new EnduringConsentRequest(authFlow, startDate, endDate, period, amount,
    ConsentDetail.TypeEnum.Enduring);
    
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
#### Westpac
Westpac requires you to specify which account of the customers to debit.

The available selection of accounts is supplied to you in the consent response of an Authorised Westpac consent object, and the ID of the selected account in supplied here.
```csharp
var request = new PaymentRequest(consentId, null, accountReferenceId);

var paymentResponse = client.CreateWestpacPayment(request);
```
#### Retrieval
```csharp
var payment = client.GetPayment(paymentId);
```

### Refunds
#### Account Number Refund
```csharp
var request = new AccountNumberRefundRequest(paymentId, RefundDetail.TypeEnum.AccountNumber);

var refundResponse = client.CreateRefund(request);
```
#### Full Refund (Not yet implemented)
```csharp
var pcr = new Pcr(particulars, code, reference);
var request = new FullRefundRequest(paymentId, pcr, redirectUri, RefundDetail.TypeEnum.FullRefund);

var refundResponse = client.CreateRefund(request);
```
#### Partial Refund (Not yet implemented)
```csharp
var amount = new Amount(total, Amount.CurrencyEnum.NZD);
var pcr = new Pcr(particulars, code, reference);
var request = new PartialRefundRequest(paymentId, amount pcr, redirectUri, RefundDetail.TypeEnum.PartialRefund);

var refundResponse = client.CreateRefund(request);
```
#### Retrieval
```csharp
var refund = client.GetRefund(refundId);
```