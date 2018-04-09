namespace Lykke.Service.PayAuth.Client.Models.Employees
{
    /// <summary>
    /// Merchant employee update credentials details.
    /// </summary>
    public class UpdateCredentialsModel
    {
        /// <summary>
        /// Gets or sets an employee id.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets an merchant id.
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets an email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a password.
        /// </summary>
        public string Password { get; set; }
    }
}
