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
            return _tableStorage.InsertThrowConflict(Create(payauth));
        }
        private static string GetPartitionKey()
            => "PayAuth";

        private static string GetRowKey(string str)
            => str.ToLower();
        private static PayAuthEntity Create(IPayAuth payauth)
        {
            return new PayAuthEntity
            {
                PartitionKey = payauth.ClientId,
                RowKey = GetRowKey(payauth.SystemId),
                ApiKey = payauth.ApiKey,
                Certificate = payauth.Certificate
            };
        }

        public async Task<IPayAuth> GetAsync(string clientId, string systemId)
        {
            return await _tableStorage.GetDataAsync(clientId, GetRowKey(systemId));
        }
    }
}
