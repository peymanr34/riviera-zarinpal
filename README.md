[![Nuget Version][nuget-badge]][nuget]
[![Nuget Version Pre][nuget-badge-pre]][nuget-pre]
[![Nuget Downloads][nuget-badge-dl]][nuget]

# Riviera.ZarinPal
Unofficial implementation of ZarinPal API for .NET

## Installing
You can install this package by entering the following command into your `Package Manager Console`:
```powershell
Install-Package Riviera.ZarinPal -PreRelease
```

*Note:* This package requires **.NET Core 3.1** or **.NET 5.0+**.

## How to use
This step by step example shows how to use `Riviera.ZarinPal` in your **ASP.NET Core** MVC project.

### 1. Configure Service
Add following code at the top of your `Startup.cs` file:

```csharp
using Riviera.ZarinPal.Extensions;
```

Then configure settings in `ConfigureServices`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddZarinPal(options =>
    {
        // TODO: Use app secrets instead of hard-coding MerchantId.
        // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
        options.MerchantId = "your-merchant-id";
        // Redirects to a 'test' payment gate.
        // TODO: Remove or set 'false' in production.
        options.IsDevelopment = true;
    });
}
```

### 2. Use in controller
Add following code at the top of your controller:

```csharp
using Riviera.ZarinPal;
```

Then add `IZarinPalService` to your controller's constructor:

```csharp
public class HomeController : Controller
{
    private readonly IZarinPalService _zarinpal;

    public HomeController(IZarinPalService zarinpal)
    {
        _zarinpal = zarinpal;
    }
}
```

### 3. Prepare and Send
```csharp
[Route("send")]
public async Task<IActionResult> Send()
{
    long amount = 1000;
    string description = "This is a test payment";
    string callbackUrl = "https://localhost:5001/get";

    var request = await _zarinpal.RequestPayment(amount, description, new Uri(callbackUrl));

    if (request.IsSuccess)
    {
        return Redirect(request.PaymentUri.AbsoluteUri);
    }

    return Content($"Failed, Error code: {request.Status}");
}
```

### 4. Verify
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
    var response = await _zarinpal.VerifyPayment(amount, authority);

    // Check if transaction was successful.
    if (response.IsSuccess)
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
[nuget-pre]: https://www.nuget.org/packages/Riviera.ZarinPal/absoluteLatest
[nuget-badge]: https://img.shields.io/nuget/v/Riviera.ZarinPal.svg?label=Release
[nuget-badge-pre]: https://img.shields.io/nuget/vpre/Riviera.ZarinPal?label=Preview
[nuget-badge-dl]: https://img.shields.io/nuget/dt/Riviera.ZarinPal?label=Downloads&color=red