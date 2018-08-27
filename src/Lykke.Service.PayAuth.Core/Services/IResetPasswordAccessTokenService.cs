using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Services
{
    public interface IResetPasswordAccessTokenService
    {
        Task<ResetPasswordAccessToken> CreateAsync(string employeeId, string merchantId);

        Task<ResetPasswordAccessToken> GetByPublicIdAsync(string publicId);

        Task<ResetPasswordAccessToken> RedeemAsync(string publicId);
    }
}
