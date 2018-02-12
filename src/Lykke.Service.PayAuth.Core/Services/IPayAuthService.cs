using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Services
{
    public interface IPayAuthService
    {
        Task AddAsync(IPayAuth payauth);

        Task<IPayAuth> GetAsync(string clientId, string systemId);
    }
}
