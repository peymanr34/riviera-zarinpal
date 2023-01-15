namespace Riviera.ZarinPal.V4
{
    /// <summary>
    /// The <see cref="ZarinPalOptions"/> Version 4 class.
    /// </summary>
    public class ZarinPalOptions
    {
        /// <summary>
        /// Gets or sets the merchant id.
        /// </summary>
        public string? MerchantId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is in development enviroment.
        /// </summary>
        public bool IsDevelopment { get; set; }
    }
}
