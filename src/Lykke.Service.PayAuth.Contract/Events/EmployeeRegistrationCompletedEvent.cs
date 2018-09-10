using ProtoBuf;

namespace Lykke.Service.PayAuth.Contract.Events
{
    [ProtoContract]
    public class EmployeeRegistrationCompletedEvent
    {
        [ProtoMember(1, IsRequired = true)]
        public string Id { get; set; }
        
        [ProtoMember(2, IsRequired = true)]
        public string ResetPasswordUrl { get; set; }
    }
}
