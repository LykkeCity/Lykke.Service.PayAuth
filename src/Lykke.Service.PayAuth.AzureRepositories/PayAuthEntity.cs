using Lykke.Service.PayAuth.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PayAuth.AzureRepositories
{
    public class PayAuthEntity : TableEntity, IPayAuth
    {
        public static class ByClientId
        {
            public static string GeneratePartitionKey(string clientId)
            {
                return clientId;
            }

            public static string GenerateRowKey(string systemId)
            {
                return systemId;
            }

            public static PayAuthEntity Create(IPayAuth src)
            {
                return new PayAuthEntity
                {
                    ApiKey = src.ApiKey,
                    Certificate = src.Certificate,
                    ClientId = src.ClientId,
                    SystemId = src.SystemId,
                    PartitionKey = GeneratePartitionKey(src.ClientId),
                    RowKey = GenerateRowKey(src.SystemId)
                };
            }
        }

        public string SystemId { get; set; }

        public string ClientId { get; set; }

        public string ApiKey { get; set; }

        public string Certificate { get; set; }
    }
}
