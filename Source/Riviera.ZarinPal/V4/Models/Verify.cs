namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The transaction verification response.
    /// </summary>
    public class Verify : Response
    {
        /// <summary>
        /// Gets or sets the card hash (SHA256).
        /// </summary>
        [JsonPropertyName("card_hash")]
        public string? CardHash { get; set; }

        /// <summary>
        /// Gets or sets the masked card number.
        /// </summary>
        [JsonPropertyName("card_pan")]
        public string? CardPan { get; set; }

        /// <summary>
        /// Gets or sets the reference id.
        /// </summary>
        [JsonPropertyName("ref_id")]
        public long RefId { get; set; }

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
    }
}
