namespace Riviera.ZarinPal.V4.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The error class.
    /// </summary>
    public class Error : Response
    {
        /// <summary>
        /// Gets or sets the validations.
        /// </summary>
        [JsonPropertyName("validations")]
        [JsonConverter(typeof(ValidationsConverter))]
        public List<KeyValuePair<string, string>>? Validations { get; set; }
    }
}
