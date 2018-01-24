namespace Lykke.Service.PayAuth.Core.Domain
{
    public interface IEmployeeCredentials
    {
        string Email { get; set; }
        
        string Password { get; set; }
        
        string Salt { get; set; }
    }
}
