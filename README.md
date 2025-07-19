# Riviera.ZarinPal
Unofficial implementation of the ZarinPal API for .NET

[![NuGet Version][nuget-shield]][nuget]
[![NuGet Downloads][nuget-shield-dl]][nuget]

## Installation
You can install this package via the `Package Manager Console` in Visual Studio.

```powershell
Install-Package Riviera.ZarinPal -PreRelease
```

## Choosing the version
Currently there are two **ZarinPal REST API** versions to choose from:

- [**Version 1:**](#version-1)
This is the [older version][docs-v1] of their REST API with basic functionality and simple responses.
- [**Version 4:**](#version-4)
This is the [newer version][docs-v4] of their REST API with more features.

[docs-v1]:https://github.com/ZarinPal-Lab/Documentation-PaymentGateway
[docs-v4]: https://www.zarinpal.com/docs

## Version 4
To use the `ZarinPalService` (v4) you need to register it via the `IServiceCollection`.

```csharp
// using Riviera.ZarinPal.V4;

builder.Services.Configure<ZarinPalOptions>(options => builder.Configuration.GetSection("ZarinPal").Bind(options));
builder.Services.AddHttpClient<ZarinPalService>();
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
// using Riviera.ZarinPal.V4;

public class HomeController : Controller
{
    private readonly ZarinPalService _zarinpal;

    public HomeController(ZarinPalService zarinpal)
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

    var verify = new NewVerify
    {
        Amount = 1000,
        Authority = authority,
    };

    var result = await _zarinpal.VerifyPaymentAsync(verify);

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

### Payment Status
You can get the status of a transaction via the `GetPaymentStatusAsync` method.

```csharp
// using Riviera.ZarinPal.V4.Models;

var request = new NewPaymentStatus
{
    Authority = "authority",
};

var result = await _zarinpal.GetPaymentStatusAsync(request);
```


### Refund
You can refund a transaction via the `RefundAsync` method.

- **Authority** is the transaction unique id.
- **AccessToken** is a personal access token that can be generated in your user account page.

```csharp
// using Riviera.ZarinPal.V4.Models;

var refund = new NewRefund
{
    Authority = "authority",
};

var result = await _zarinpal.RefundAsync(refund);
```

## Version 1
To use `ZarinPalService` (v1) you need to register it via the `IServiceCollection`.

```csharp
// using Riviera.ZarinPal.V1;

builder.Services.Configure<ZarinPalOptions>(options => builder.Configuration.GetSection("ZarinPal").Bind(options));
builder.Services.AddHttpClient<ZarinPalService>();
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
// using Riviera.ZarinPal.V1;

public class HomeController : Controller
{
    private readonly ZarinPalService _zarinpal;

    public HomeController(ZarinPalService zarinpal)
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
