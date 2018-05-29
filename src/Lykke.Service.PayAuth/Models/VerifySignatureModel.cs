using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models
{
    public class VerifySignatureModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string SystemId { get; set; }

        [Required]
        public string ClientId { get; set; }
    }
}
