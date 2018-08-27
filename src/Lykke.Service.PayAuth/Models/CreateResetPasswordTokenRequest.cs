using System.ComponentModel.DataAnnotations;
using LykkePay.Common.Validation;

namespace Lykke.Service.PayAuth.Models
{
    /// <summary>
    /// New reset password token request details
    /// </summary>
    public class CreateResetPasswordTokenRequest
    {
        /// <summary>
        /// Gets or sets employee id
        /// </summary>
        [Required]
        [RowKey]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets merchant id
        /// </summary>
        [Required]
        public string MerchantId { get; set; }
    }
}
