# Riviera.ZarinPal
Unofficial implementation of ZarinPal API for .NET

[![Nuget Version][nuget-shield]][nuget]
[![Nuget Downloads][nuget-shield-dl]][nuget]

## Installing the NuGet Package
You can install this package by entering the following command into your `Package Manager Console`:

```powershell
Install-Package Riviera.ZarinPal -PreRelease
```

*Note:* This package requires **.NET Core 3.1** or **.NET 5.0+**.

## Startup Configuration
To use `ZarinPalService` you need to register it in your `Startup.cs` class.

```csharp
// using Riviera.ZarinPal;

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

## Controller Configuration
After registering the service, you need to add it to your controller.

```csharp
// using Riviera.ZarinPal;

public class HomeController : Controller
{
    private readonly IZarinPalService _zarinpal;

    public HomeController(IZarinPalService zarinpal)
    {
        _zarinpal = zarinpal;
    }
}
```

## Requesting a payment
You can request a payment via `RequestPayment` method.

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

## Verifying a payment
You can verify the transaction via `VerifyPayment` method.

> This action was our **Callback Url** in the `RequestPayment` method.
> After the transaction was completed, the payment provider will redirect to this action. 

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
[nuget-shield]: https://img.shields.io/nuget/v/Riviera.ZarinPal.svg?label=Release
[nuget-shield-dl]: https://img.shields.io/nuget/dt/Riviera.ZarinPal?label=Downloads&color=red