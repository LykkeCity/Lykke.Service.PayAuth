namespace Lykke.Service.PayAuth.Core.Domain
{
    public class EmployeeCredentials : IEmployeeCredentials
    {
        public string EmployeeId { get; set; }
        public string MerchantId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string PinCode { get; set; }
        public bool ForcePasswordUpdate { get; set; }
        public bool ForcePinUpdate { get; set; }
        public bool ForceEmailConfirmation { get; set; }
    }
}
