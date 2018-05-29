using Lykke.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Lykke.Service.PayAuth.Core
{
    public class SecurityHelper : ISecurityHelper
    {
        public SecurityErrorType CheckRequest(string strToSign, string clientId, string sign, string publicKey, string apiKey)
        {

            if (string.IsNullOrEmpty(clientId))
            {
                return SecurityErrorType.MerchantUnknown;
            }

            if (string.IsNullOrEmpty(sign))
            {
                return SecurityErrorType.SignEmpty;
            }

            strToSign = $"{apiKey}{strToSign}";

            var cert = GetCertificate(publicKey);

            var rsa = cert.GetRSAPublicKey();
            bool isCorrect = false;
            try
            {
                isCorrect = rsa.VerifyData(Encoding.UTF8.GetBytes(strToSign), Convert.FromBase64String(sign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch { }


            return isCorrect ? SecurityErrorType.Ok : SecurityErrorType.SignIncorrect;
        }

        private X509Certificate2 GetCertificate(string cert)
        {
            cert = cert.Replace("-----BEGIN CERTIFICATE-----", string.Empty);
            cert = cert.Replace("-----END CERTIFICATE-----", string.Empty);
            cert = new Regex("(?is)\\s+").Replace(cert, string.Empty);
            return new X509Certificate2(Convert.FromBase64String(cert));
        }

    }
}
