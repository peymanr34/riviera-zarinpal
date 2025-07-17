namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The <see cref="NewRefund"/> class.
    /// </summary>
    public class NewRefund
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
    }
}
