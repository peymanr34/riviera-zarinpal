namespace Riviera.ZarinPal.V4.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The pending (to be verified) transactions response.
    /// </summary>
    public class Pending : Response
    {
        /// <summary>
        /// Gets or sets the payments list.
        /// </summary>
        [JsonPropertyName("authorities")]
        public IList<PendingAuthority>? Authorities { get; set; }
    }
}
