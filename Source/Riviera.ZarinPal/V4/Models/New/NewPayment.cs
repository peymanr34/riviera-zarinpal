namespace Riviera.ZarinPal.V4.Models
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The <see cref="NewPayment"/> class.
    /// </summary>
    public class NewPayment
    {
        /// <summary>
        /// Gets or sets the currency unit (IRR or IRT).
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        /// <summary>
        /// Gets or sets the transaction description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the callback url.
        /// </summary>
        [JsonPropertyName("callback_url")]
        public Uri? CallbackUri { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [JsonPropertyName("mobile")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the metadata for additional information.
        /// </summary>
        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Metadata? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the order id.
        /// </summary>
        [JsonPropertyName("order_id")]
        public string? OrderId { get; set; }

        /// <summary>
        /// Gets or sets the merchant id.
        /// </summary>
        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }

        /// <summary>
        /// Creates a new payment.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="description">The transaction description.</param>
        /// <param name="callbackUri">The transaction callback uri.</param>
        /// <param name="emailAddress">The user email address.</param>
        /// <param name="phoneNumber">The user phone number.</param>
        /// <returns>A new instance of the <see cref="NewPayment"/> class.</returns>
        public static NewPayment Create(long amount, string description, Uri? callbackUri = null, string? emailAddress = null, string? phoneNumber = null)
        {
            return new NewPayment
            {
                Amount = amount,
                Description = description,
                CallbackUri = callbackUri,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
            };
        }
    }
}
