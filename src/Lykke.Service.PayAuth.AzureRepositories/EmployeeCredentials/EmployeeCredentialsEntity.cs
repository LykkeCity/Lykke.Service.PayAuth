using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials
{
    public class EmployeeCredentialsEntity : TableEntity
    {
        public EmployeeCredentialsEntity()
        {
        }

        public EmployeeCredentialsEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }
        
        public string EmployeeId { get; set; }
        public string MerchantId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
