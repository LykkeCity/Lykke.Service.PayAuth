using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lykke.Service.PayAuth.Models
{
    public class NewPayAuthModel
    {
        [Required]
        public string SystemId { get; set; }
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string Certificate { get; set; }
    }
}
