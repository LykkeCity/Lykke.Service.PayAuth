using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Repositories;
using Lykke.Service.PayAuth.AzureRepositories.Extensions;

namespace Lykke.Service.PayAuth.AzureRepositories
{
    public class PayAuthRepository : IPayAuthRepository
    {
        private readonly INoSQLTableStorage<PayAuthEntity> _tableStorage;
        public PayAuthRepository(INoSQLTableStorage<PayAuthEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public Task AddAsync(IPayAuth payauth)
        {
            return _tableStorage.InsertThrowConflict(PayAuthEntity.Create(payauth));
        }
        public async Task<IPayAuth> GetAsync(string clientId, string systemId)
        {
            return await _tableStorage.GetDataAsync(clientId, systemId);
        }
    }
}
