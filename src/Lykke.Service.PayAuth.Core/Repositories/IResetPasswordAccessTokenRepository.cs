using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.Core.Repositories
{
    public interface IResetPasswordAccessTokenRepository
    {
        Task<ResetPasswordAccessToken> CreateAsync(ResetPasswordAccessToken token);

        Task<ResetPasswordAccessToken> GetByPublicIdAsync(string publicId);

        Task<ResetPasswordAccessToken> UpdateAsync(ResetPasswordAccessToken src);
    }
}
