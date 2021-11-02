namespace Riviera.ZarinPal.Models
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents reponse of a payment request.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Gets a value indicating whether payment request has been succeeded.
        /// </summary>
        [Obsolete("This property will be removed in the next version. Please use the IsSuccess extension methods.")]
        public bool IsSuccess => this.IsSuccess();

        /// <summary>
        /// Gets or sets unique refrence id of the transaction.
        /// </summary>
        [JsonPropertyName("Authority")]
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets an status code of the transaction.
        /// </summary>
        [JsonPropertyName("Status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets an uri to start the payment process.
        /// </summary>
        [JsonIgnore]
        public Uri? PaymentUri { get; internal set; }
    }
}