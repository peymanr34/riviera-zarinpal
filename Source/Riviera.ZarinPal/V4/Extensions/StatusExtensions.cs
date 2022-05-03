namespace Riviera.ZarinPal.V4.Extensions
{
    using Riviera.ZarinPal.V4.Models;

    /// <summary>
    /// The version 4 status extensions.
    /// </summary>
    public static class StatusExtensions
    {
        /// <summary>
        /// Gets a value indicating whether payment request was successful.
        /// </summary>
        /// <param name="response">The response of a payment request.</param>
        /// <returns>true if the payment request was successful.</returns>
        public static bool IsSuccess(this Payment response)
        {
            return response.Code == 100;
        }

        /// <summary>
        /// Gets a value indicating whether transaction was successful.
        /// </summary>
        /// <param name="response">The response of a transaction verification request.</param>
        /// <returns>true if the transaction was successful.</returns>
        public static bool IsSuccess(this Verify response)
        {
            return (response.Code == 100) || (response.Code == 101);
        }
    }
}
