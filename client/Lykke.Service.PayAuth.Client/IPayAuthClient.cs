using Lykke.Service.PayAuth.Client.Models;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models.Employees;
using System.Threading;
using Lykke.Service.PayAuth.Client.Models.GenerateRsaKeys;

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
        /// Updates api key
        /// </summary>
        Task UpdateApiKeyAsync(UpdateApiKeyRequest request);

        /// <summary>
        /// Get pay auth information
        /// </summary>
        /// <param name="merchantId">Merchant id</param>
        Task<PayAuthInformationResponse> GetPayAuthInformationAsync(string merchantId);

        /// <summary>
        /// Generates rsa keys
        /// </summary>
        Task<GenerateRsaKeysResponse> GenerateRsaKeysAsync(GenerateRsaKeysRequest request);

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
        /// Validates employee password
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <param name="password">The employee password.</param>
        /// <returns>The validation result.</returns>
        Task<ValidateResultModel> ValidatePasswordAsync(string email, string password);

        /// <summary>
        /// Updates password hash
        /// </summary>
        /// <param name="model">Password hash update details</param>
        /// <returns></returns>
        Task UpdatePasswordHashAsync(UpdatePasswordHashModel model);

        /// <summary>
        /// Turns on passwordUpdate marker
        /// </summary>
        /// <param name="model">Enforce password details</param>
        /// <returns></returns>
        Task EnforceCredentialsUpdateAsync(EnforceCredentialsUpdateModel model);

        /// <summary>
        /// Validates employee pin
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <param name="pin">The employee pin</param>
        /// <returns></returns>
        Task<ValidateResultModel> ValidatePinAsync(string email, string pin);

        /// <summary>
        /// Updates pin hash
        /// </summary>
        /// <param name="model">Pin hash update details</param>
        /// <returns></returns>
        Task UpdatePinHashAsync(UpdatePinHashModel model);

        /// <summary>
        /// Creates new reset password token for employee
        /// </summary>
        /// <param name="request">Request details</param>
        /// <returns></returns>
        Task<ResetPasswordTokenModel> CreateResetPasswordTokenAsync(CreateResetPasswordTokenRequest request);

        /// <summary>
        /// Returns reset password access token details
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task<ResetPasswordTokenModel> GetResetPasswordTokenByPublicIdAsync(string publicId);

        /// <summary>
        /// Redeems reset password token
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task<ResetPasswordTokenModel> RedeemResetPasswordTokenAsync(string publicId);
    }
}
