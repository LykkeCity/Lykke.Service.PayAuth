using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.PayAuth.Core;

namespace Lykke.Service.PayAuth.Models
{
    public class PayAuthModel
    {
        [Required]
        [DefaultValue(LykkePayConstants.DefaultSystemId)]
        public string SystemId { get; set; }

        [Required]
        public string ClientId { get; set; }

        public string ApiKey { get; set; }

        public string Certificate { get; set; }
    }
}
