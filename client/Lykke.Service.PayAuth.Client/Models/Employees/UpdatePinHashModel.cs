namespace Lykke.Service.PayAuth.Client.Models.Employees
{
    /// <summary>
    /// Update pin model details
    /// </summary>
    public class UpdatePinHashModel
    {
        /// <summary>
        /// Gets or sets an email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password hash
        /// </summary>
        public string PinHash { get; set; }
    }
}
