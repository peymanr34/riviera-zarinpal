﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Xunit;

namespace Riviera.ZarinPal.V1.Tests
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
                MerchantId = "something-that-does-not-matter",
                IsDevelopment = true
            };

            var service = new ZarinPalService(httpClient, Options.Create(options));

            await Assert.ThrowsAsync<ArgumentNullException>("callbackUri", async () =>
            {
                await service.RequestPaymentAsync(1000, "From tests", callbackUri: null);
            });
        }
    }
}