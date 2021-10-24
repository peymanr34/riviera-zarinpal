namespace Riviera.ZarinPal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Riviera.ZarinPal.Models;

    /// <summary>
    /// The <see cref="IZarinPalService"></see> interface.
    /// </summary>
    public interface IZarinPalService
    {
        /// <summary>
        /// Request a new payment from the service.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="description">The transaction description.</param>
        /// <param name="callbackUri">
        /// An Url to redirect to after tansaction has been compleated.
        /// * Optional only if you have specified callbackUri in options.
        /// </param>
        /// <param name="emailAddress">The user email address.</param>
        /// <param name="phoneNumber">The user phone number.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentException">Thrown when callbackUri has not been suplied via options or passed to the method.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<PaymentResponse> RequestPaymentAsync(long amount, string description, Uri? callbackUri = null, string? emailAddress = null, string? phoneNumber = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify the payment transaction.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="authority">The unique refrence id of the transaction.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<VerifyResponse> VerifyPaymentAsync(long amount, string authority, CancellationToken cancellationToken = default);

        /// <summary>
        /// Indicates whether the status returned from the service was successful.
        /// </summary>
        /// <param name="status">The status code returned from the service.</param>
        /// <returns>true if the value parameter was indicating success.</returns>
        bool IsStatusValid(string status);

        /// <summary>
        /// Indicates whether the status returned from the service was not successful.
        /// </summary>
        /// <param name="status">The status code returned from the service.</param>
        /// <returns>true if the value parameter was indicating failure.</returns>
        bool IsStatusNotValid(string status);
    }
}