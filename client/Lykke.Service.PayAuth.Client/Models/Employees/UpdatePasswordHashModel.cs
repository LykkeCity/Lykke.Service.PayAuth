namespace Lykke.Service.PayAuth.Client.Models.Employees
{
    /// <summary>
    /// Update password model details
    /// </summary>
    public class UpdatePasswordHashModel
    {
        /// <summary>
        /// Gets or sets an email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password hash
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
