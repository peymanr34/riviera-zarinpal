namespace Riviera.ZarinPal
{
    using System;

    /// <summary>
    /// The <see cref="ZarinPalOptions"/> class.
    /// </summary>
    public class ZarinPalOptions
    {
        /// <summary>
        /// Gets or sets the acceptor unique merchant id.
        /// </summary>
        public string? MerchantId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is on testing mode.
        /// If true no actual transactions will accure.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Zarin Gate is enabled.
        /// If enabled you will be redirected to direct payment gate instead of default Web Gate.
        /// </summary>
        public bool IsZarinGateEnabled { get; set; }

        /// <summary>
        /// Gets or sets the default callback Uri (optional).
        /// </summary>
        public Uri? DefaultCallbackUri { get; set; }
    }
}