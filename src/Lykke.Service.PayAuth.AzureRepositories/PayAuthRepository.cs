using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Repositories;

namespace Lykke.Service.PayAuth.AzureRepositories
{
    public class PayAuthRepository : IPayAuthRepository
    {
        private readonly INoSQLTableStorage<PayAuthEntity> _tableStorage;

        public PayAuthRepository(INoSQLTableStorage<PayAuthEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task AddAsync(IPayAuth payauth)
        {
            var newItem = PayAuthEntity.ByClientId.Create(payauth);

            await _tableStorage.InsertOrMergeAsync(newItem);
        }

        public async Task UpdateAsync(IPayAuth payauth)
        {
            await _tableStorage.MergeAsync(
                PayAuthEntity.ByClientId.GeneratePartitionKey(payauth.ClientId),
                PayAuthEntity.ByClientId.GenerateRowKey(payauth.SystemId),
                entity =>
            {
                if (!string.IsNullOrEmpty(payauth.ApiKey))
                {
                    entity.ApiKey = payauth.ApiKey;
                }

                if (!string.IsNullOrEmpty(payauth.Certificate))
                {
                    entity.Certificate = payauth.Certificate;
                }

                return entity;
            });
        }

        public async Task<IPayAuth> GetAsync(string clientId, string systemId)
        {
            return await _tableStorage.GetDataAsync(
                PayAuthEntity.ByClientId.GeneratePartitionKey(clientId),
                PayAuthEntity.ByClientId.GenerateRowKey(systemId));
        }
    }
}
