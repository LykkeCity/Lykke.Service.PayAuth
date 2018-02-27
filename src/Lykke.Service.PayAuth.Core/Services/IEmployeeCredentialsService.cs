using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Services
{
    public interface IEmployeeCredentialsService
    {
        Task RegisterAsync(IEmployeeCredentials employeeCredentials);
        Task UpdateAsync(IEmployeeCredentials employeeCredentials);

        Task<IEmployeeCredentials> ValidateAsync(string email, string password);
        
        Task DeleteAsync(string email);
    }
}
