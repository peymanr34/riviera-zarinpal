using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Riviera.ZarinPal.Tests
{
    public class RequestTests
    {
        [Fact]
        public async Task ShouldThrowExceptionWhenCallbackIsNotSet()
        {
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

            string json = JsonSerializer.Serialize(new
            {
                Status = 100,
                Authority = "000000000000000000000000000000012345"
            });

            var mockHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var mockHttpClient = new HttpClient(mockHttpMessageHandler);

            httpClientFactoryMock.CreateClient()
                .Returns(mockHttpClient);

            var options = Options.Create(new ZarinPalOptions()
            {
                DefaultCallbackUri = null,
                MerchantId = "something-that-does-not-matter",
                IsDevelopment = true
            });

            var service = new ZarinPalService(mockHttpClient, options);

            await Assert.ThrowsAsync<ArgumentException>("callbackUri", async () =>
            {
                await service.RequestPayment(1000, "From tests", callbackUri: null);
            });
        }
    }
}