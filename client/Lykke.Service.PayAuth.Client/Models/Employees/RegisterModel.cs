namespace Lykke.Service.PayAuth.Client.Models.Employees
{
    /// <summary>
    /// Merchant employee registration details.
    /// </summary>
    public class RegisterModel
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

        /// <summary>
        /// Gets or sets an attribute of requiring the email confirmation
        /// </summary>
        public bool ForceEmailConfirmation { get; set; }
    }
}
