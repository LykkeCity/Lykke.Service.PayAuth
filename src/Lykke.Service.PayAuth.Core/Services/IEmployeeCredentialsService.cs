using System.Threading.Tasks;

namespace Lykke.Service.PayAuth.Core.Services
{
    public interface IEmployeeCredentialsService
    {
        Task RegisterAsync(string email, string password);
        
        Task<bool> ValidateAsync(string email, string password);
        
        Task DeleteAsync(string email);
    }
}
