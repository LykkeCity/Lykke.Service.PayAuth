using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayAuth.Models
{
    public class UpdateApiKeyModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ApiKey { get; set; }
    }
}
