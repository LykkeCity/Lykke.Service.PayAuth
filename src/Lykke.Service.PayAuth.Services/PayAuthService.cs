using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Core.Repositories;

namespace Lykke.Service.PayAuth.Services
{

    public class PayAuthService : IPayAuthService
    {
        private readonly IPayAuthRepository _payAuthRepository;
        public PayAuthService(
            IPayAuthRepository payAuthRepository
        )
        {
            _payAuthRepository = payAuthRepository;
        }

        public async Task AddAsync(IPayAuth payauth)
        {
            //if (await _payAuthRepository.GetAsync(payauth.Id) != null)
            //{
            //    throw new ServiceException("already exists.");
            //}
            await _payAuthRepository.AddAsync(payauth);
        }

        public async Task<IPayAuth> GetAsync(string clientId, string systemId)
        {
            return await _payAuthRepository.GetAsync(clientId, systemId);
        }
    }
}
