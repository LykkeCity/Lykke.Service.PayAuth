using Lykke.Service.PayAuth.Client.Models;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models.Employees;
using System.Threading;

namespace Lykke.Service.PayAuth.Client
{
    /// <summary>
    /// HTTP client for pay auth service.
    /// </summary>
    public interface IPayAuthClient
    {
        /// <summary>
        /// Registers new client.
        /// </summary>
        /// <param name="request">The registration details.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validates signature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<SignatureValidationResponse> VerifyAsync(VerifyRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Registers an employee credentials.
        /// </summary>
        /// <param name="model">The registration details.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        Task RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates an employee credentials.
        /// </summary>
        /// <param name="model">The employee credentials.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        Task UpdateAsync(UpdateCredentialsModel model, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validates employee credentials.
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <param name="password">The employee password.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The validation result.</returns>
        Task<ValidateResultModel> ValidateAsync(string email, string password, CancellationToken cancellationToken = default(CancellationToken));
    }
}
