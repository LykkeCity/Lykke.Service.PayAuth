using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Repositories
{
    public interface IPayAuthRepository
    {
        Task AddAsync(IPayAuth payauth);
        Task UpdateAsync(IPayAuth payauth);
        Task<IPayAuth> GetAsync(string clientId, string systemId);
    }
}
