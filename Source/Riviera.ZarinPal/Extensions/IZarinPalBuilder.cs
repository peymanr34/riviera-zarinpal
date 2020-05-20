namespace Riviera.ZarinPal.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The <see cref="IZarinPalBuilder"/> interface.
    /// </summary>
    public interface IZarinPalBuilder
    {
        /// <summary>
        /// Gets the services that being configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}