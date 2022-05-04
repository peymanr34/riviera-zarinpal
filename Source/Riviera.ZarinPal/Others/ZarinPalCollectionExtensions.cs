namespace Riviera.ZarinPal
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for setting up ZarinPal services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ZarinPalCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="ZarinPalService"/> and related services to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="options">The <see cref="ZarinPalOptions"/> to configure.</param>
        public static void AddZarinPal(this IServiceCollection services, Action<ZarinPalOptions> options)
        {
            services.Configure(options);
            services.AddHttpClient<ZarinPalService>();
        }
    }
}