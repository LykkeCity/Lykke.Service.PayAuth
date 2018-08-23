using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models.GenerateRsaKeys
{
    public class GenerateRsaKeysModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientDisplayName { get; set; }
    }
}
