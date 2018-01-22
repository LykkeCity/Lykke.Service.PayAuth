
using Lykke.Service.PayAuth.Client.Models;
using System.Threading.Tasks;

namespace Lykke.Service.PayAuth.Client
{
    public interface IPayAuthClient
    {
        Task RegisterAsync(RegisterRequest request);
        Task<string> VerifyAsync(VerifyRequest request);
    }
}
