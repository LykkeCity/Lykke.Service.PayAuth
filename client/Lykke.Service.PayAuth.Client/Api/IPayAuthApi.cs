using System.Threading.Tasks;
using Refit;
using Lykke.Service.PayAuth.Client.Models;
using System.Threading;

namespace Lykke.Service.PayAuth.Client.Api
{
    internal interface IPayAuthApi
    {
        [Post("/api/register")]
        Task RegisterAsync([Body] RegisterRequest request, CancellationToken cancellationToken = default(CancellationToken));

        [Post("/api/verify/signature")]
        Task<SignatureValidationResponse> VerifyAsync([Body] VerifyRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
