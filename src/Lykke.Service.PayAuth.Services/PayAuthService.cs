using System;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Exceptions;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Core.Repositories;

namespace Lykke.Service.PayAuth.Services
{
    public class PayAuthService : IPayAuthService
    {
        private readonly IPayAuthRepository _payAuthRepository;

        public PayAuthService(
            IPayAuthRepository payAuthRepository)
        {
            _payAuthRepository = payAuthRepository ?? throw new ArgumentNullException(nameof(payAuthRepository));
        }

        public async Task AddAsync(IPayAuth payauth)
        {
            await _payAuthRepository.AddAsync(payauth);
        }

        public async Task UpdateAsync(IPayAuth payauth)
        {
            var client = await _payAuthRepository.GetAsync(payauth.ClientId, payauth.SystemId);

            if (client == null)
                throw new ClientNotFoundException(payauth.ClientId);

            await _payAuthRepository.UpdateAsync(payauth);
        }

        public async Task<IPayAuth> GetAsync(string clientId, string systemId)
        {
            var client = await _payAuthRepository.GetAsync(clientId, systemId);

            if(client == null)
                throw new ClientNotFoundException(clientId);

            return client;
        }
    }
}
