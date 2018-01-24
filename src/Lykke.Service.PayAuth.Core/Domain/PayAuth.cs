using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PayAuth.Core.Domain
{
    public class PayAuth : IPayAuth
    {
        public string Id { get; set; }
        public string SystemId { get; set; }
        public string ClientId { get; set; }
        public string Certificate { get; set; }
        public string ApiKey { get; set; }
    }
}
