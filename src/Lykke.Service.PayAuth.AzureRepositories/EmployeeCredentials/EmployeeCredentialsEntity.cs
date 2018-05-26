using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class EmployeeCredentialsEntity : AzureTableEntity
    {
        private bool _updatePassword;

        public string EmployeeId { get; set; }

        public string MerchantId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string PinCode { get; set; }

        public bool UpdatePassword
        {
            get => _updatePassword;
            set
            {
                _updatePassword = value;
                MarkValueTypePropertyAsDirty(nameof(UpdatePassword));
            }
        }
    }
}
