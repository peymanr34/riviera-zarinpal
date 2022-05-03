namespace Riviera.ZarinPal.V4.Models
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The pending (to be verified) transaction class.
    /// </summary>
    public class PendingAuthority
    {
        /// <summary>
        /// Gets or sets the authority.
        /// </summary>
        [JsonPropertyName("authority")]
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the callback url.
        /// </summary>
        [JsonPropertyName("callback_url")]
        public string? CallbackUrl { get; set; }

        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        [JsonPropertyName("referer")]
        public string? Referer { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
