namespace Riviera.ZarinPal.V1.Models
{
    using System;
    using System.Text.Json.Serialization;
    using Riviera.ZarinPal;

    /// <summary>
    /// Represents response of a transaction verification request.
    /// </summary>
    public class VerifyResponse
    {
        /// <summary>
        /// Gets a value indicating whether transaction has been succeeded.
        /// </summary>
        [Obsolete("This property will be removed in the next version. Please use the IsSuccess extension methods.")]
        public bool IsSuccess => this.IsSuccess();

        /// <summary>
        /// Gets or sets the payment refrence id.
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