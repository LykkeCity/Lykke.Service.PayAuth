namespace Lykke.Service.PayAuth.Client.Models
{
    /// <summary>
    /// New reset password token request details
    /// </summary>
    public class CreateResetPasswordTokenRequest
    {
        /// <summary>
        /// Gets or sets employee id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets merchant id
        /// </summary>
        public string MerchantId { get; set; }
    }
}
