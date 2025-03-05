namespace Riviera.ZarinPal.V1
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Riviera.ZarinPal.V1.Models;

    /// <summary>
    /// The <see cref="ZarinPalService"/> class.
    /// </summary>
    public class ZarinPalService
    {
        private readonly HttpClient _httpClient;
        private readonly ZarinPalOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZarinPalService"/> class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/>.</param>
        /// <param name="options">An instance of <see cref="ZarinPalOptions"/>.</param>
        public ZarinPalService(HttpClient httpClient, IOptions<ZarinPalOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(options.Value.MerchantId))
            {
                throw new ArgumentException($"'{nameof(options.Value.MerchantId)}' has not been configured.", nameof(options));
            }
        }

        /// <summary>
        /// Request a new payment.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="description">The transaction description.</param>
        /// <param name="callbackUri">An Url to redirect to after transaction has been completed.</param>
        /// <param name="emailAddress">The user email address.</param>
        /// <param name="phoneNumber">The user phone number.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{PaymentResponse}"/> representing the asynchronous operation.</returns>
        public async Task<PaymentResponse?> RequestPaymentAsync(long amount, string description, Uri callbackUri, string? emailAddress = null, string? phoneNumber = null, CancellationToken cancellationToken = default)
        {
            if (callbackUri is null)
            {
                throw new ArgumentNullException(nameof(callbackUri));
            }

            var request = new PaymentRequest
            {
                Amount = amount,
                Description = description,
                PhoneNumber = phoneNumber,
                EmailAddress = emailAddress,
                CallbackUri = callbackUri,
                MerchantId = _options.MerchantId,
            };

            var result = await PostJsonAsync<PaymentResponse>("PaymentRequest.json", request, cancellationToken);

            if (!string.IsNullOrWhiteSpace(result?.Authority))
            {
                result.PaymentUri = GetPaymentGateUrl(result.Authority);
            }

            return result;
        }

        /// <summary>
        /// Verify the payment transaction.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="authority">The unique reference id of the transaction.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{VerifyResponse}"/> representing the asynchronous operation.</returns>
        public async Task<VerifyResponse?> VerifyPaymentAsync(long amount, string authority, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(authority))
            {
                throw new ArgumentException($"'{nameof(authority)}' cannot be null or whitespace", nameof(authority));
            }

            var request = new VerifyRequest
            {
                Amount = amount,
                Authority = authority,
                MerchantId = _options.MerchantId,
            };

            return await PostJsonAsync<VerifyResponse>("PaymentVerification.json", request, cancellationToken);
        }

        /// <summary>
        /// Indicates whether the status returned from the service was successful.
        /// </summary>
        /// <param name="status">The status code returned from the service.</param>
        /// <returns>true if the value parameter was indicating success.</returns>
        public bool IsStatusValid(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException($"'{nameof(status)}' cannot be null or whitespace.", nameof(status));
            }

            if (status.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if (status.Equals("NOK", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            throw new NotSupportedException("The response query string is not valid.");
        }

        /// <summary>
        /// Indicates whether the status returned from the service was not successful.
        /// </summary>
        /// <param name="status">The status code returned from the service.</param>
        /// <returns>true if the value parameter was indicating failure.</returns>
        public bool IsStatusNotValid(string status)
        {
            return !IsStatusValid(status);
        }

        private async Task<T?> PostJsonAsync<T>(string relativeUrl, object value, CancellationToken cancellationToken = default)
        {
            string prefix = _options.IsDevelopment ? "sandbox" : "www";
            string url = $"https://{prefix}.zarinpal.com/pg/rest/WebGate/{relativeUrl}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            string requestJson = JsonSerializer.Serialize(value);
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            return await SendRequestAsync<T>(request, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            string json = await response.Content
#if NET5_0_OR_GREATER
                .ReadAsStringAsync(cancellationToken)
#else
                .ReadAsStringAsync()
#endif
                .ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(json);
        }

        private Uri GetPaymentGateUrl(string id)
        {
            string prefix = _options.IsDevelopment ? "sandbox" : "www";
            string url = $"https://{prefix}.zarinpal.com/pg/StartPay/{id}";

            if (_options.IsZarinGateEnabled)
            {
                url += "/ZarinGate";
            }

            return new Uri(url);
        }
    }
}