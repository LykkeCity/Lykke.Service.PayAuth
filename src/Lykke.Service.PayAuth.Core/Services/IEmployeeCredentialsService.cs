using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Services
{
    public interface IEmployeeCredentialsService
    {
        Task RegisterAsync(IEmployeeCredentials employeeCredentials);

        Task UpdateAsync(IEmployeeCredentials employeeCredentials);

        Task UpdatePasswordHashAsync(string email, string hash);

        Task UpdatePinHashAsync(string email, string hash);

        Task EnforceCredentialsUpdateAsync(string email);

        Task<IEmployeeCredentials> ValidatePasswordAsync(string email, string password);

        Task<IEmployeeCredentials> ValidatePinAsync(string email, string pin);

        Task DeleteAsync(string email);

        string CalculateHash(string source, string salt);
    }
}
