using System.Threading.Tasks;
using Refit;
using Lykke.Service.PayAuth.Client.Models;
using System.Threading;
using Lykke.Service.PayAuth.Client.Models.GenerateRsaKeys;

namespace Lykke.Service.PayAuth.Client.Api
{
    internal interface IPayAuthApi
    {
        [Post("/api/register")]
        Task RegisterAsync([Body] RegisterRequest request, CancellationToken cancellationToken = default(CancellationToken));

        [Get("/api/payauth")]
        Task<PayAuthInformationResponse> GetPayAuthInformationAsync(string merchantId);

        [Put("/api/payauth")]
        Task UpdateApiKeyAsync([Body] UpdateApiKeyRequest request);

        [Post("/api/generatersakeys")]
        Task<GenerateRsaKeysResponse> GenerateRsaKeysAsync([Body] GenerateRsaKeysRequest request);

        [Post("/api/verify/signature")]
        Task<SignatureValidationResponse> VerifyAsync([Body] VerifyRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
