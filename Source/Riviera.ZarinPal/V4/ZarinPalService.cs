namespace Riviera.ZarinPal.V4
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Riviera.ZarinPal.V4.Models;

    /// <summary>
    /// The <see cref="ZarinPalService"/> Version 4 class.
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
        }

        /// <summary>
        /// Request a new payment.
        /// </summary>
        /// <param name="request">The payment request details.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation.</returns>
        public async Task<Result<Payment>?> RequestPaymentAsync(NewPayment request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.MerchantId ??= _options.MerchantId;

            if (string.IsNullOrWhiteSpace(request.MerchantId))
            {
                throw new ArgumentException($"'{nameof(request.MerchantId)}' has not been configured. Configure it via 'options' or use the '{nameof(request.MerchantId)}' parameter.", nameof(request));
            }

            var result = await PostJsonAsync<Result<Payment>>("payment/request.json", request, null, cancellationToken)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(result?.Data?.Authority))
            {
                result.Data.PaymentUri = GetPaymentGateUrl(result.Data.Authority);
            }

            return result;
        }

        /// <summary>
        /// Verify the payment transaction.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="authority">The unique reference id of the transaction.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation.</returns>
        public async Task<Result<Verify>?> VerifyPaymentAsync(long amount, string authority, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(authority))
            {
                throw new ArgumentException($"'{nameof(authority)}' cannot be null or whitespace", nameof(authority));
            }

            var request = new
            {
                amount = amount,
                authority = authority,
                merchant_id = _options.MerchantId,
            };

            return await PostJsonAsync<Result<Verify>>("payment/verify.json", request, null, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of payments that are not verified.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation.</returns>
        public async Task<Result<Pending>?> GetPendingPaymentsAsync(CancellationToken cancellationToken = default)
        {
            var request = new
            {
                merchant_id = _options.MerchantId,
            };

            return await PostJsonAsync<Result<Pending>>("payment/unVerified.json", request, null, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Refunds a transaction.
        /// </summary>
        /// <param name="authority">The unique reference id of the transaction.</param>
        /// <param name="token">The access token.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation.</returns>
        public async Task<Result<Refund>?> RefundAsync(string authority, string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException($"'{nameof(token)}' cannot be null or empty.", nameof(token));
            }

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentException($"'{nameof(authority)}' cannot be null or empty.", nameof(authority));
            }

            var request = new
            {
                authority = authority,
                merchant_id = _options.MerchantId,
            };

            return await PostJsonAsync<Result<Refund>>("payment/refund.json", request, token, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Indicates whether the status query string returned from the service was successful.
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

            throw new NotSupportedException("The response query string was not valid.");
        }

        /// <summary>
        /// Indicates whether the status returned from the service was not successful.
        /// </summary>
        /// <param name="status">The status query string returned from the service.</param>
        /// <returns>true if the value parameter was indicating failure.</returns>
        public bool IsStatusNotValid(string status)
        {
            return !IsStatusValid(status);
        }

        private async Task<T?> PostJsonAsync<T>(string relativeUrl, object value, string? token = null, CancellationToken cancellationToken = default)
        {
            string prefix = _options.IsDevelopment ? "sandbox" : "payment";
            string url = $"https://{prefix}.zarinpal.com/pg/v4/{relativeUrl}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            string requestJson = JsonSerializer.Serialize(value);
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            return await SendRequestAsync<T>(request, token, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request, string? token = null, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            // In some cases, server returns html.
            if (response.Content.Headers.ContentType?.MediaType == "text/html")
            {
                return default;
            }

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
            string prefix = _options.IsDevelopment ? "sandbox" : "payment";
            string url = $"https://{prefix}.zarinpal.com/pg/StartPay/{id}";

            return new Uri(url);
        }
    }
}
