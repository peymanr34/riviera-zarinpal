namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The <see cref="Metadata"/> class for providing additional data to the request.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        [JsonPropertyName("card_pan")]
        public int? CardPan { get; set; }
    }
}
