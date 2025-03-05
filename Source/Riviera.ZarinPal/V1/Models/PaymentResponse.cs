namespace Riviera.ZarinPal.V1.Models
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents response of a payment request.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Gets or sets the unique reference id of the transaction.
        /// </summary>
        [JsonPropertyName("Authority")]
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the status code of the transaction.
        /// </summary>
        [JsonPropertyName("Status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets the uri to start the payment process.
        /// </summary>
        [JsonIgnore]
        public Uri? PaymentUri { get; internal set; }
    }
}