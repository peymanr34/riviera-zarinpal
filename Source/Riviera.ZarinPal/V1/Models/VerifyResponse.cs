namespace Riviera.ZarinPal.V1.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents response of a transaction verification request.
    /// </summary>
    public class VerifyResponse
    {
        /// <summary>
        /// Gets or sets the payment reference id.
        /// </summary>
        [JsonPropertyName("RefID")]
        public long RefId { get; set; }

        /// <summary>
        /// Gets or sets the payment status code.
        /// </summary>
        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }
}