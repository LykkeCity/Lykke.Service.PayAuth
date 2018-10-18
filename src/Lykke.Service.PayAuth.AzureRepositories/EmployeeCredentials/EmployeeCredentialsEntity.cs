using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.PayAuth.Core.Domain;

namespace Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class EmployeeCredentialsEntity : AzureTableEntity, IEmployeeCredentials
    {
        private bool _forcePasswordUpdate;

        private bool _forcePinUpdate;

        private bool _forceEmailConfirmation;

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

        public bool ForceEmailConfirmation
        {
            get => _forceEmailConfirmation;
            set
            {
                _forceEmailConfirmation = value;
                MarkValueTypePropertyAsDirty(nameof(ForceEmailConfirmation));
            }
        }
    }
}
