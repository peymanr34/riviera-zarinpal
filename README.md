# Riviera.ZarinPal
Unofficial implementation of the ZarinPal API for .NET

[![NuGet Version][nuget-shield]][nuget]
[![NuGet Downloads][nuget-shield-dl]][nuget]

## Installation
You can install this package via the `Package Manager Console` in Visual Studio.

```powershell
Install-Package Riviera.ZarinPal -PreRelease
```

*Note:* This package requires **.NET 6.0+**.

## Choosing the version
Currently there are two **ZarinPal REST API** versions to choose from.

- [**Version 1:**](#version-1)
This is the [older version][docs-v1] of their REST API with basic functionality and simple responses.
- [**Version 4:**](#version-4)
This is the [newer version][docs-v4] of their REST API with more features. Since this endpoint is buggy, it is opt-in.

[docs-v1]:https://github.com/ZarinPal-Lab/Documentation-PaymentGateway
[docs-v4]: https://next.zarinpal.com/paymentGateway

## Version 4
To use the `ZarinPalService` (v4) you need to register it via the `IServiceCollection`.

```csharp
builder.Services.Configure<Riviera.ZarinPal.V4.ZarinPalOptions>(options => builder.Configuration.GetSection("ZarinPal").Bind(options));
builder.Services.AddHttpClient<Riviera.ZarinPal.V4.ZarinPalService>();
```

Then, add the following options to your `appsettings.Development.json` file.

```json
"ZarinPal": {
  "MerchantId": "your-merchant-id",
  "IsDevelopment": true
}
```

After registering the service, you need to add it to your controller.

```csharp
public class HomeController : Controller
{
    private readonly Riviera.ZarinPal.V4.ZarinPalService _zarinpal;

    public HomeController(Riviera.ZarinPal.V4.ZarinPalService zarinpal)
    {
        _zarinpal = zarinpal;
    }
}
```

### Requesting a payment
You can request a payment via the `RequestPaymentAsync` method.

- **Amount** is the transaction amount.
- **Description** is a short description about the transaction.
- **CallbackUri** is the url to redirect to after the transaction. 

```csharp
// using Riviera.ZarinPal.V4.Models;

[Route("send")]
public async Task<IActionResult> Send()
{
    var payment = new NewPayment
    {
        Amount = 1000,
        Description = "This is a test payment",
        CallbackUri = new Uri("https://localhost:5001/get"),
        Currency = "IRR",
    };

    var result = await _zarinpal.RequestPaymentAsync(payment);

    if (result?.Data?.PaymentUri != null && result.Data.Code == 100)
    {
        return Redirect(result.Data.PaymentUri.AbsoluteUri);
    }

    return Content($"Failed, Error {result.Error.Code}: {result.Error.Message}");
}
```

### Verifying the transaction
You can verify the transaction via the `VerifyPaymentAsync` method.

> This action was our **CallbackUri** in the `RequestPaymentAsync` method.

After the transaction is completed, the payment provider will redirect to this action. 

```csharp
// using Riviera.ZarinPal.V4.Models;

[Route("get")]
public async Task<IActionResult> Get()
{
    // Get Status and Authority and show error if not available.
    if (!Request.Query.TryGetValue("Status", out var status) ||
        !Request.Query.TryGetValue("Authority", out var authority))
    {
        return Content("Bad request.");
    }

    // Check if query string status is valid.
    if (_zarinpal.IsStatusValid(status) == false)
    {
        return Content("Failed.");
    }

    long amount = 1000;
    var result = await _zarinpal.VerifyPaymentAsync(amount, authority);

    // Check if transaction was successful.
    if (result?.Data != null && (result.Data.Code == 100 || result.Data.Code == 101))
    {
        return Content($"Success, RefId: {result.Data.RefId}");
    }

    // Show unsuccessful transaction with code.
    return Content($"Failed, Error {result.Data.Code}: {result.Error.Message}");
}
```

### Pending Payments
You can get the transactions that has not been verified via the `GetPendingPaymentsAsync` method.

```csharp
var result = await _zarinpal.GetPendingPaymentsAsync();
```

### Refund
You can refund a transaction via the `RefundAsync` method.

- **Authority** is the transaction unique id.
- **AccessToken** is a personal access token that can be generated in your user account page.

```csharp
var result = await _zarinpal.RefundAsync("authority", "accessToken");
```

### Sandbox Issues
Since the release of the v4 (in 2020), the service returns a html response if you try to use the "Sandbox" feature.

Because of this bug, we have a check for html responses and you may see the following exception:

```log
An unhandled exception occurred while processing the request.
Exception: Server returned an invalid value (html).
```

To avoid this exception, you may need to remove the `"IsDevelopment": true` option (or set it to `false`) and test with the production endpoint.

> You can also call the customer support and ask them to fix this issue!

## Version 1
To use `ZarinPalService` (v1) you need to register it in your `Startup.cs` class.

```csharp
// using Riviera.ZarinPal.V1;

services.AddZarinPal(options =>
{
    // TODO: Use app secrets instead of hard-coding MerchantId.
    // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
    options.MerchantId = "your-merchant-id";
    
    // Redirects to a 'test' payment gate.
    // TODO: Remove or set 'false' in production.
    options.IsDevelopment = true;
});
```

After registering the service, you need to add it to your controller.

```csharp
public class HomeController : Controller
{
    private readonly Riviera.ZarinPal.V1.ZarinPalService _zarinpal;

    public HomeController(Riviera.ZarinPal.V1.ZarinPalService zarinpal)
    {
        _zarinpal = zarinpal;
    }
}
```

### Requesting a payment
You can request a payment via the `RequestPaymentAsync` method.

- **Amount** is the transaction amount.
- **Description** is a short description about the transaction.
- **Callback Url** is the url to redirect to after the transaction. 

```csharp
[Route("send")]
public async Task<IActionResult> Send()
{
    long amount = 1000;
    string description = "This is a test payment";
    string callbackUrl = "https://localhost:5001/get";

    var request = await _zarinpal.RequestPaymentAsync(amount, description, new Uri(callbackUrl));

    if (request.IsSuccess())
    {
        return Redirect(request.PaymentUri.AbsoluteUri);
    }

    return Content($"Failed, Error code: {request.Status}");
}
```

### Verifying a transaction
You can verify the transaction via the `VerifyPaymentAsync` method.

> This action was our **Callback Url** in the `RequestPayment` method.

After the transaction is completed, the payment provider will redirect to this action. 

```csharp
[Route("get")]
public async Task<IActionResult> Get()
{
    // Get Status and Authority and show error if not available.
    if (!Request.Query.TryGetValue("Status", out var status) ||
        !Request.Query.TryGetValue("Authority", out var authority))
    {
        return Content("Bad request.");
    }

    // Check if query string status is valid.
    if (_zarinpal.IsStatusValid(status) == false)
    {
        return Content("Failed.");
    }

    long amount = 1000;
    var response = await _zarinpal.VerifyPaymentAsync(amount, authority);

    // Check if transaction was successful.
    if (response.IsSuccess())
    {
        return Content($"Success, RefId: {response.RefId}");
    }

    // Show unsuccessful transaction with code.
    return Content($"Failed, Error code: {response.Status}");
}
```

## License
This project is licensed under the [MIT License](LICENSE).

[nuget]: https://www.nuget.org/packages/Riviera.ZarinPal
[nuget-shield]: https://img.shields.io/nuget/v/Riviera.ZarinPal.svg?label=NuGet
[nuget-shield-dl]: https://img.shields.io/nuget/dt/Riviera.ZarinPal?label=Downloads&color=red
