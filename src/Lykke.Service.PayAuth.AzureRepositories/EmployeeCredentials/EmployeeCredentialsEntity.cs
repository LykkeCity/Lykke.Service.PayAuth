using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class EmployeeCredentialsEntity : AzureTableEntity
    {
        private bool _forcePasswordUpdate;

        private bool _forcePinUpdate;

        public string EmployeeId { get; set; }

        public string MerchantId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string PinCode { get; set; }

        public bool ForcePasswordUpdate
        {
            get => _forcePasswordUpdate;
            set
            {
                _forcePasswordUpdate = value;
                MarkValueTypePropertyAsDirty(nameof(ForcePasswordUpdate));
            }
        }

        public bool ForcePinUpdate
        {
            get => _forcePinUpdate;
            set
            {
                _forcePinUpdate = value;
                MarkValueTypePropertyAsDirty(nameof(ForcePinUpdate));
            }
        }
    }
}
