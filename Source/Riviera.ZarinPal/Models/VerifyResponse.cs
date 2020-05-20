namespace Riviera.ZarinPal.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents response of a transaction verification request.
    /// </summary>
    public class VerifyResponse
    {
        /// <summary>
        /// Gets a value indicating whether transaction has been succeeded.
        /// </summary>
        public bool IsSuccess => (Status == 100) || (Status == 101);

        /// <summary>
        /// Gets or sets  payment refrence id.
        /// </summary>
        [JsonPropertyName("RefID")]
        public int RefId { get; set; }

        /// <summary>
        /// Gets or sets payment status code.
        /// </summary>
        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }
}