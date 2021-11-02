namespace Riviera.ZarinPal
{
    using Riviera.ZarinPal.Models;

    /// <summary>
    /// The status extensions.
    /// </summary>
    public static class StatusExtensions
    {
        /// <summary>
        /// Gets a value indicating whether payment request was successful.
        /// </summary>
        /// <param name="response">The response of a payment request.</param>
        /// <returns>true if the payment request was successful.</returns>
        public static bool IsSuccess(this PaymentResponse response)
        {
            return response.Status == 100;
        }

        /// <summary>
        /// Gets a value indicating whether transaction was successful.
        /// </summary>
        /// <param name="response">The response of a transaction verification request.</param>
        /// <returns>true if the transaction was successful.</returns>
        public static bool IsSuccess(this VerifyResponse response)
        {
            return (response.Status == 100) || (response.Status == 101);
        }
    }
}
