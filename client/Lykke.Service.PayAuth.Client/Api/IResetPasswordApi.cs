using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models;
using Refit;

namespace Lykke.Service.PayAuth.Client.Api
{
    internal interface IResetPasswordApi
    {
        [Get("/api/resetPasswordToken/{publicId}")]
        Task<ResetPasswordTokenModel> GetByPublicIdAsync(string publicId);

        [Put("/api/resetPasswordToken/{publicId}")]
        Task<ResetPasswordTokenModel> RedeemAsync(string publicId);
    }
}
