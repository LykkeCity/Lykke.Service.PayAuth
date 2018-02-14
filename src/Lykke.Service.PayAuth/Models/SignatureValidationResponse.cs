using Lykke.Common.Entities.Security;

namespace Lykke.Service.PayAuth.Models
{
    public class SignatureValidationResponse
    {
        public string Description { get; set; }
        public SecurityErrorType ErrorType { get; set; }
    }
}
