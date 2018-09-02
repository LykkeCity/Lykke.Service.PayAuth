using ProtoBuf;

namespace Lykke.Service.PayAuth.Contract.Commands
{
    [ProtoContract]
    public class GeneratePasswordResetTokenCommand
    {
        [ProtoMember(1, IsRequired = true)]
        public string EmployeeId { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public string MerchantId { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public bool IsNewEmployee { get; set; }
    }
}
