namespace Riviera.ZarinPal.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents transaction verification request.
    /// </summary>
    public class VerifyRequest
    {
        /// <summary>
        /// Gets the acceptor unique merchant id.
        /// </summary>
        [JsonPropertyName("MerchantID")]
        public string? MerchantId { get; internal set; }

        /// <summary>
        /// Gets or sets the amount of the transaction.
        /// </summary>
        [JsonPropertyName("Amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the unique reference id.
        /// </summary>
        [JsonPropertyName("Authority")]
        public string? Authority { get; set; }
    }
}