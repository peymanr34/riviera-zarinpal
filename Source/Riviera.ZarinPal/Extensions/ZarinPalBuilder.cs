namespace Riviera.ZarinPal.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Used to configure ZarinPal.
    /// </summary>
    public class ZarinPalBuilder : IZarinPalBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZarinPalBuilder"/> class.
        /// </summary>
        /// <param name="services">The services being configured.</param>
        public ZarinPalBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc/>
        public virtual IServiceCollection Services { get; }
    }
}