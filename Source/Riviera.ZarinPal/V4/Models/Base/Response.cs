namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The base response class.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
