using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Service.PayAuth.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PayAuth.AzureRepositories
{
    public class PayAuthEntity : TableEntity, IPayAuth
    {
        public string Id => RowKey;
        public string SystemId { get; set; }
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string Certificate { get; set; }

        private static string GetRowKey(string str)
            => str.ToLower();
        public static PayAuthEntity Create(IPayAuth payauth)
        {
            return new PayAuthEntity
            {
                PartitionKey = payauth.ClientId,
                RowKey = GetRowKey(payauth.Id),
                ApiKey = payauth.ApiKey,
                Certificate = payauth.Certificate,
                SystemId = payauth.SystemId
            };
        }
    }
}
