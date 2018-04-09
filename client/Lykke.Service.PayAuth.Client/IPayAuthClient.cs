using Lykke.Service.PayAuth.Client.Models;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models.Employees;

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
        Task RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Validates signature
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SignatureValidationResponse> VerifyAsync(VerifyRequest request);

        /// <summary>
        /// Registers an employee credentials.
        /// </summary>
        /// <param name="model">The registration details.</param>
        Task RegisterAsync(RegisterModel model);
        
        /// <summary>
        /// Updates an employee credentials.
        /// </summary>
        /// <param name="model">The employee credentials.</param>
        Task UpdateAsync(UpdateCredentialsModel model);

        /// <summary>
        /// Validates employee credentials.
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <param name="password">The employee password.</param>
        /// <returns>The validation result.</returns>
        Task<ValidateResultModel> ValidateAsync(string email, string password);
    }
}
