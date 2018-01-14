using System.Threading.Tasks;

namespace Lykke.Service.SignIn.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}