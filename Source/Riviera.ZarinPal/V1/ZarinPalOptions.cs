namespace Riviera.ZarinPal.V1
{
    /// <summary>
    /// The <see cref="ZarinPalOptions"/> class.
    /// </summary>
    public class ZarinPalOptions
    {
        /// <summary>
        /// Gets or sets the merchant id.
        /// </summary>
        public string? MerchantId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is in development environment.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Zarin Gate' feature is enabled.
        /// </summary>
        public bool IsZarinGateEnabled { get; set; }
    }
}