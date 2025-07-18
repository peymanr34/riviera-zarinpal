namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The <see cref="NewVerify"/> class.
    /// </summary>
    public class NewVerify
    {
        /// <summary>
        /// Gets or sets the merchant id.
        /// </summary>
        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the authority.
        /// </summary>
        [JsonPropertyName("authority")]
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }
    }
}
