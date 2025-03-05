namespace Riviera.ZarinPal.V1
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
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

        private string UrlPrefix
            => _options.IsDevelopment ? "sandbox" : "www";

        private Uri RequestPaymentUrl
            => new Uri($"https://{UrlPrefix}.zarinpal.com/pg/rest/WebGate/PaymentRequest.json");

        private Uri VerifyPaymentUrl
            => new Uri($"https://{UrlPrefix}.zarinpal.com/pg/rest/WebGate/PaymentVerification.json");

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
        public async Task<PaymentResponse> RequestPaymentAsync(long amount, string description, Uri callbackUri, string? emailAddress = null, string? phoneNumber = null, CancellationToken cancellationToken = default)
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

            using var response = await _httpClient
                .PostAsJsonAsync(RequestPaymentUrl, request, cancellationToken)
                .ConfigureAwait(false);

            var result = await response.Content
                .ReadFromJsonAsync<PaymentResponse>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (result is null)
            {
                throw new Exception("Error while getting response. No Response.");
            }

            if (!string.IsNullOrWhiteSpace(result.Authority))
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
        public async Task<VerifyResponse> VerifyPaymentAsync(long amount, string authority, CancellationToken cancellationToken = default)
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

            using var response = await _httpClient
                .PostAsJsonAsync(VerifyPaymentUrl, request, cancellationToken)
                .ConfigureAwait(false);

            return await response.Content
                .ReadFromJsonAsync<VerifyResponse>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
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

        private Uri GetPaymentGateUrl(string id)
        {
            if (_options.IsZarinGateEnabled)
            {
                return new Uri($"https://{UrlPrefix}.zarinpal.com/pg/StartPay/{id}/ZarinGate");
            }

            return new Uri($"https://{UrlPrefix}.zarinpal.com/pg/StartPay/{id}");
        }
    }
}