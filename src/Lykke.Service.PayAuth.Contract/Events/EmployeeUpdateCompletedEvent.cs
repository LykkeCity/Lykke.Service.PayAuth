using ProtoBuf;

namespace Lykke.Service.PayAuth.Contract.Events
{
    [ProtoContract]
    public class EmployeeUpdateCompletedEvent
    {
        [ProtoMember(1, IsRequired = true)]
        public string Id { get; set; }

        [ProtoMember(2, IsRequired = false)]
        public string ResetPasswordUrl { get; set; }
    }
}
