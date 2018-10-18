using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Repositories
{
    public interface IEmployeeCredentialsRepository
    {
        Task<IEmployeeCredentials> GetAsync(string email);
        
        Task InsertOrReplaceAsync(IEmployeeCredentials credentials);

        Task<IEmployeeCredentials> UpdateEmailConfirmationAttributeAsync(string email);
        
        Task DeleteAsync(string email);
    }
}
