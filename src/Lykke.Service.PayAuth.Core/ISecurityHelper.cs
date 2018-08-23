using Lykke.Common.Entities.Security;

namespace Lykke.Service.PayAuth.Core
{
    public interface ISecurityHelper
    {
        SecurityErrorType CheckRequest(string strToSign, string clientId, string sign, string publicKey,
            string apiKey);

        (string PrivateKey, string PublicKey) GenerateRsaKeys(string companyName);
    }
}
