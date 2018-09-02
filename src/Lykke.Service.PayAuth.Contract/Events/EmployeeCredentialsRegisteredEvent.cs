using ProtoBuf;

namespace Lykke.Service.PayAuth.Contract.Events
{
    [ProtoContract]
    public class EmployeeCredentialsRegisteredEvent
    {
        [ProtoMember(1, IsRequired = true)]
        public string EmployeeId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public string MerchantId { get; set; }
    }
}
