namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The refund response class.
    /// </summary>
    public class Refund : Response
    {
        /// <summary>
        /// Gets or sets the reference id.
        /// </summary>
        [JsonPropertyName("ref_id")]
        public long RefId { get; set; }

        /// <summary>
        /// Gets or sets the session (id?).
        /// </summary>
        [JsonPropertyName("session")]
        public long Session { get; set; }

        /// <summary>
        /// Gets or sets the refund account number.
        /// </summary>
        [JsonPropertyName("iban")]
        public string? Iban { get; set; }
    }
}
