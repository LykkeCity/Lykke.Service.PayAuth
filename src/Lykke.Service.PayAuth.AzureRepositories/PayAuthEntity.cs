using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Service.PayAuth.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PayAuth.AzureRepositories
{
    public class PayAuthEntity : TableEntity, IPayAuth
    {
        public string SystemId => RowKey;
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string Certificate { get; set; }
    }
}
