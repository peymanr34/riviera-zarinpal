namespace Riviera.ZarinPal.V1
{
    using System;

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
        /// Gets or sets a value indicating whether the application is in development enviroment.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "Zarin Gate" is enabled.
        /// </summary>
        public bool IsZarinGateEnabled { get; set; }

        /// <summary>
        /// Gets or sets the default callback Uri (optional).
        /// </summary>
        [Obsolete("This option will be removed in the future versions. Please use the 'CallbackUri' property on the request.")]
        public Uri? DefaultCallbackUri { get; set; }
    }
}