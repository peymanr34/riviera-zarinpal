namespace Riviera.ZarinPal
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Riviera.ZarinPal.Models;

    /// <summary>
    /// The <see cref="ZarinPalService"/> class.
    /// </summary>
    public class ZarinPalService : IZarinPalService
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
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.Value.MerchantId))
            {
                throw new ArgumentException($"'{nameof(options.Value.MerchantId)}' has not been configured.", nameof(options));
            }

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
        }

        private string UrlPrefix
            => _options.IsDevelopment ? "sandbox" : "www";

        private Uri RequestPaymentUrl
            => new Uri($"https://{UrlPrefix}.zarinpal.com/pg/rest/WebGate/PaymentRequest.json");

        private Uri VerifyPaymentUrl
            => new Uri($"https://{UrlPrefix}.zarinpal.com/pg/rest/WebGate/PaymentVerification.json");

        /// <inheritdoc/>
        public async Task<PaymentResponse> RequestPayment(long amount, string description, Uri? callbackUri = null, string? emailAddress = null, string? phoneNumber = null, CancellationToken cancellationToken = default)
        {
            var actualCallbackUri = callbackUri is null ? _options.DefaultCallbackUri : callbackUri;

            if (actualCallbackUri is null)
            {
                throw new ArgumentException($"'{nameof(_options.DefaultCallbackUri)}' has not been configured. To avoid this, configure it via 'options' or use the '{nameof(callbackUri)}' parameter.", nameof(callbackUri));
            }

            var request = new PaymentRequest()
            {
                Amount = amount,
                Description = description,
                PhoneNumber = phoneNumber,
                EmailAddress = emailAddress,
                CallbackUri = actualCallbackUri,
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

        /// <inheritdoc/>
        public async Task<VerifyResponse> VerifyPayment(long amount, string authority, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(authority))
            {
                throw new ArgumentException($"'{nameof(authority)}' cannot be null or whitespace", nameof(authority));
            }

            var request = new VerifyRequest()
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

        /// <inheritdoc/>
        public bool IsStatusValid(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException(paramName: nameof(status), message: "Status cannot be null");
            }

            if (status.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if (status.Equals("NOK", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            throw new NotSupportedException("Response query string is not valid.");
        }

        /// <inheritdoc/>
        public bool IsStatusNotValid(string status) => !IsStatusValid(status);

        private Uri GetPaymentGateUrl(string id) => _options.IsZarinGateEnabled
            ? new Uri($"https://{UrlPrefix}.zarinpal.com/pg/StartPay/{id}/ZarinGate")
            : new Uri($"https://{UrlPrefix}.zarinpal.com/pg/StartPay/{id}");
    }
}