using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PayAuth.Client.Models
{
    public class VerifyRequest
    {
        public string Text { get; set; }
        public string Signature { get; set; }
        public string SystemId { get; set; }
        public string ClientId { get; set; }
    }
}
