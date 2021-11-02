namespace Riviera.ZarinPal.Models
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a payment request to the service.
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Gets the acceptor unique merchant id.
        /// </summary>
        [JsonPropertyName("MerchantID")]
        public string? MerchantId { get; internal set; }

        /// <summary>
        /// Gets or sets the url to redirect after the transaction.
        /// </summary>
        [JsonPropertyName("CallbackURL")]
        public Uri? CallbackUri { get; set; }

        /// <summary>
        /// Gets or sets the transaction description.
        /// </summary>
        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the amount of the transaction.
        /// </summary>
        [JsonPropertyName("Amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the user phone number.
        /// </summary>
        [JsonPropertyName("Mobile")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user email address.
        /// </summary>
        [JsonPropertyName("Email")]
        public string? EmailAddress { get; set; }
    }
}