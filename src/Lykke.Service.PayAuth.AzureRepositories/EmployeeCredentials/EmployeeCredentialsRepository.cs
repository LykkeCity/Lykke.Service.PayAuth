using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Repositories;

namespace Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials
{
    public class EmployeeCredentialsRepository : IEmployeeCredentialsRepository
    {
        private readonly INoSQLTableStorage<EmployeeCredentialsEntity> _storage;

        public EmployeeCredentialsRepository(INoSQLTableStorage<EmployeeCredentialsEntity> storage)
        {
            _storage = storage;
        }
        
        public async Task<IEmployeeCredentials> GetAsync(string email)
        {
            EmployeeCredentialsEntity entity = await _storage.GetDataAsync(GetPartitionKey(email), GetRowKey());

            if (entity == null)
                return null;
            
            return new Core.Domain.EmployeeCredentials
            {
                MerchantId = entity.MerchantId,
                EmployeeId = entity.EmployeeId,
                Email = entity.Email,
                Password = entity.Password,
                Salt = entity.Salt,
                PinCode = entity.PinCode,
                ForcePasswordUpdate = entity.ForcePasswordUpdate,
                ForcePinUpdate = entity.ForcePinUpdate,
                ForceEmailConfirmation = entity.ForceEmailConfirmation
            };
        }

        public async Task InsertOrReplaceAsync(IEmployeeCredentials credentials)
        {
            await _storage.InsertOrReplaceAsync(new EmployeeCredentialsEntity
            {
                PartitionKey = GetPartitionKey(credentials.Email),
                RowKey = GetRowKey(),
                MerchantId = credentials.MerchantId,
                EmployeeId = credentials.EmployeeId,
                Email = credentials.Email,
                Password = credentials.Password,
                Salt = credentials.Salt,
                PinCode = credentials.PinCode,
                ForcePasswordUpdate = credentials.ForcePasswordUpdate,
                ForcePinUpdate = credentials.ForcePinUpdate,
                ForceEmailConfirmation = credentials.ForceEmailConfirmation
            });
        }

        public async Task<IEmployeeCredentials> UpdateEmailConfirmationAttributeAsync(string email)
        {
            var result = await _storage.MergeAsync(GetPartitionKey(email), GetRowKey(), entity =>
            {
                entity.ForceEmailConfirmation = false;
                return entity;
            });

            return result;
        }

        public async Task DeleteAsync(string email)
        {
            await _storage.DeleteAsync(GetPartitionKey(email), GetRowKey());
        }
        
        private static string GetPartitionKey(string email)
            => email.ToLower().Trim();

        private static string GetRowKey()
            => "password";
    }
}
