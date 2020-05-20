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
        /// Request a new peyment from Service.
        /// </summary>
        /// <param name="amount">Transaction amount.</param>
        /// <param name="description">Transaction description.</param>
        /// <param name="callbackUri">
        /// An Url to redirect to after tansaction has been compleated.
        /// * Optional only if you have specified callbackUri in options.
        /// </param>
        /// <param name="emailAddress">User email address.</param>
        /// <param name="phoneNumber">User phone number.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentException">Thrown when callbackUri has not been suplied via options or passed to the method.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<PaymentResponse> RequestPayment(long amount, string description, Uri? callbackUri = null, string? emailAddress = null, string? phoneNumber = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify payment transaction.
        /// </summary>
        /// <param name="amount">Transaction amount.</param>
        /// <param name="authority">Unique refrence id of the transaction.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<VerifyResponse> VerifyPayment(long amount, string authority, CancellationToken cancellationToken = default);

        /// <summary>
        /// Indicates whether the status returned from the service was successful.
        /// </summary>
        /// <param name="status">Status code returned from the service.</param>
        /// <returns>true if the value parameter was indicating success.</returns>
        bool IsStatusValid(string status);

        /// <summary>
        /// Indicates whether the status returned from the service was not successful.
        /// </summary>
        /// <param name="status">Status code returned from the service.</param>
        /// <returns>true if the value parameter was indicating failure.</returns>
        bool IsStatusNotValid(string status);
    }
}