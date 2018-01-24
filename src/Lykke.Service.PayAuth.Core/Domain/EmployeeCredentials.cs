namespace Lykke.Service.PayAuth.Core.Domain
{
    public class EmployeeCredentials : IEmployeeCredentials
    {
        public EmployeeCredentials()
        {
            
        }

        public EmployeeCredentials(string employeeId, string merchantId, string email, string password, string salt)
        {
            EmployeeId = employeeId;
            MerchantId = merchantId;
            Email = email;
            Password = password;
            Salt = salt;
        }

        public string EmployeeId { get; set; }
        public string MerchantId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
