namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The base result class.
    /// </summary>
    /// <typeparam name="T">The type of the response class.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        [JsonPropertyName("data")]
        [JsonConverter(typeof(InconsistentConverter))]
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets the response errors.
        /// </summary>
        [JsonPropertyName("errors")]
        [JsonConverter(typeof(InconsistentConverter))]
        public Error? Error { get; set; }
    }
}
