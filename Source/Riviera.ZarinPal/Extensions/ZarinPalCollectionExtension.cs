namespace Riviera.ZarinPal.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for setting up ZarinPal services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ZarinPalCollectionExtension
    {
        /// <summary>
        /// Adds the <see cref="ZarinPalService"/> and related services to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="options">The <see cref="ZarinPalOptions"/> to configure.</param>
        /// <returns>An <see cref="IZarinPalBuilder"/> that can be used to configure the client.</returns>
        public static IZarinPalBuilder AddZarinPal(this IServiceCollection services, Action<ZarinPalOptions> options)
        {
            services.Configure(options);
            services.AddHttpClient<IZarinPalService, ZarinPalService>();

            return new ZarinPalBuilder(services);
        }
    }
}