using Lykke.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PayAuth.Core
{
    public interface ISecurityHelper
    {
        SecurityErrorType CheckRequest(string strToSign, string clientId, string sign, string publicKey,
            string apiKey);
    }
}
