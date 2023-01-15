using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Riviera.ZarinPal.V4.Models;
using Xunit;

namespace Riviera.ZarinPal.V4.Tests
{
    public class RequestTests
    {
        [Fact]
        public async Task ShouldThrowExceptionWhenCallbackIsNotSet()
        {
            string json = JsonSerializer.Serialize(new
            {
                Status = 100,
                Authority = "000000000000000000000000000000012345"
            });

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*")
                .Respond("application/json", json);

            var httpClient = mockHttp.ToHttpClient();

            var options = new ZarinPalOptions
            {
                DefaultCallbackUri = null,
                MerchantId = "something-that-does-not-matter",
                IsDevelopment = true
            };

            var service = new ZarinPal.V4.ZarinPalService(httpClient, Options.Create(options));

            await Assert.ThrowsAsync<ArgumentException>("request", async () =>
            {
                var payment = new NewPayment
                {
                    Amount = 1000,
                    CallbackUri = null,
                    Description = "From tests",
                };

                await service.RequestPaymentAsync(payment);
            });
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenMerchantIdIsNotSet()
        {
            string json = JsonSerializer.Serialize(new
            {
                Status = 100,
                Authority = "000000000000000000000000000000012345"
            });

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*")
                .Respond("application/json", json);

            var httpClient = mockHttp.ToHttpClient();

            var options = new ZarinPalOptions
            {
                IsDevelopment = true,
            };

            var service = new ZarinPal.V4.ZarinPalService(httpClient, Options.Create(options));

            await Assert.ThrowsAsync<ArgumentException>("request", async () =>
            {
                var payment = new NewPayment
                {
                    Amount = 1000,
                    CallbackUri = new Uri("http://localhost:5000/"),
                };

                await service.RequestPaymentAsync(payment);
            });
        }
    }
}
