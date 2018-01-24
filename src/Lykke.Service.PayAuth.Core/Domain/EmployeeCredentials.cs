namespace Lykke.Service.PayAuth.Core.Domain
{
    public class EmployeeCredentials : IEmployeeCredentials
    {
        public EmployeeCredentials()
        {
            
        }
        
        public EmployeeCredentials(string email, string password, string salt)
        {
            Email = email;
            Password = password;
            Salt = salt;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
