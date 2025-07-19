namespace Riviera.ZarinPal.V4.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The payment status response.
    /// </summary>
    public class PaymentStatus : Response
    {
        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
