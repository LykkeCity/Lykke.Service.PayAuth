using System;

namespace Lykke.Service.PayAuth.Models
{
    /// <summary>
    /// Reset password token details
    /// </summary>
    public class ResetPasswordAccessTokenResponse
    {
        /// <summary>
        /// Gets or sets public id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets employee id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets merchant id
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets expiration date
        /// </summary>
        public DateTime ExpiresOn { get; set; }

        /// <summary>
        /// Gets or sets redeemed attribute
        /// </summary>
        public bool Redeemed { get; set; }
    }
}
