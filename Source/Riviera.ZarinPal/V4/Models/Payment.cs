namespace Riviera.ZarinPal.V4.Models
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The payment request response.
    /// </summary>
    public class Payment : Response
    {
        /// <summary>
        /// Gets or sets the authority.
        /// </summary>
        [JsonPropertyName("authority")]
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the fee type.
        /// </summary>
        [JsonPropertyName("fee_type")]
        public string? FeeType { get; set; }

        /// <summary>
        /// Gets or sets the fee amount.
        /// </summary>
        [JsonPropertyName("fee")]
        public int Fee { get; set; }

        /// <summary>
        /// Gets the uri to start the payment process.
        /// </summary>
        [JsonIgnore]
        public Uri? PaymentUri { get; internal set; }
    }
}
