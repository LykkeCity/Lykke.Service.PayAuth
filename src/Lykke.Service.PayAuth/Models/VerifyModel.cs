using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PayAuth.Models
{
    public class VerifyModel
    {
        public string Text { get; set; }
        public string Signature { get; set; }
        public string SystemId { get; set; }
        public string ClientId { get; set; }
    }
}
