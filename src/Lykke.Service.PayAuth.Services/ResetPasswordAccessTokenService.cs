using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Exceptions;
using Lykke.Service.PayAuth.Core.Repositories;
using Lykke.Service.PayAuth.Core.Services;

namespace Lykke.Service.PayAuth.Services
{
    public class ResetPasswordAccessTokenService : IResetPasswordAccessTokenService
    {
        private readonly IResetPasswordAccessTokenRepository _repository;
        private readonly TimeSpan _tokenExpirationSettings;

        public ResetPasswordAccessTokenService(
            [NotNull] IResetPasswordAccessTokenRepository repository, 
            TimeSpan tokenExpirationSettings)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tokenExpirationSettings = tokenExpirationSettings;
        }

        public async Task<ResetPasswordAccessToken> CreateAsync(string employeeId, string merchantId)
        {
            DateTime tokenExpiration = DateTime.UtcNow.Add(_tokenExpirationSettings);

            var token = ResetPasswordAccessToken.Create(employeeId, merchantId, tokenExpiration);

            return await _repository.CreateAsync(token);
        }

        public Task<ResetPasswordAccessToken> GetByPublicIdAsync(string publicId)
        {
            return _repository.GetByPublicIdAsync(publicId);
        }

        public async Task<ResetPasswordAccessToken> RedeemAsync(string publicId)
        {
            ResetPasswordAccessToken token = await _repository.GetByPublicIdAsync(publicId);

            if (token == null)
                throw new TokenNotFoundException(publicId);

            if (token.ExpiresOn <= DateTime.UtcNow)
                throw new TokenExpiredException(publicId);

            if (token.Redeemed)
                throw new TokenRedeemedException(publicId);

            token.Redeemed = true;

            try
            {
                return await _repository.UpdateAsync(token);
            }
            catch (KeyNotFoundException)
            {
                throw new TokenNotFoundException(token.PublicId);
            }
        }
    }
}
