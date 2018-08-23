using Lykke.Common.Entities.Security;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
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

        public (string PrivateKey, string PublicKey) GenerateRsaKeys(string companyName)
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            var pair = rsaKeyPairGenerator.GenerateKeyPair();

            var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(pair.Private);
            byte[] privateKeyPkcs8DerEncodedBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();

            // privateKeyPkcs8DerEncodedBytes contains PKCS#8 DER-encoded private key as a byte[]
            var pkcs8PrivateKeyInfo = PrivateKeyInfo.GetInstance(privateKeyPkcs8DerEncodedBytes);
            var pkcs1Key = RsaPrivateKeyStructure.GetInstance(pkcs8PrivateKeyInfo.ParsePrivateKey());
            byte[] privateKeyPkcs1EncodedBytes = pkcs1Key.GetEncoded();

            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN RSA PRIVATE KEY-----");
            sb.AppendLine(Convert.ToBase64String(privateKeyPkcs1EncodedBytes, Base64FormattingOptions.InsertLineBreaks));
            sb.AppendLine("-----END RSA PRIVATE KEY-----");
            var serializedPrivate = sb.ToString();

            var caName = new X509Name($"CN={companyName}");
            var caCert = GenerateCertificate(caName, caName, pair.Private, pair.Public);
            var certEncoded = caCert.GetEncoded();
            sb = new StringBuilder();
            sb.AppendLine("-----BEGIN CERTIFICATE-----");
            sb.AppendLine(Convert.ToBase64String(certEncoded, Base64FormattingOptions.InsertLineBreaks));
            sb.AppendLine("-----END CERTIFICATE-----");
            var serializedPublic = sb.ToString();

            return (serializedPrivate, serializedPublic);
        }

        private Org.BouncyCastle.X509.X509Certificate GenerateCertificate(
            X509Name issuer, X509Name subject,
            AsymmetricKeyParameter issuerPrivate,
            AsymmetricKeyParameter subjectPublic)
        {
            var signatureFactory = new Asn1SignatureFactory(
                PkcsObjectIdentifiers.Sha256WithRsaEncryption.ToString(),
                issuerPrivate);

            var certGenerator = new X509V3CertificateGenerator();
            certGenerator.SetIssuerDN(issuer);
            certGenerator.SetSubjectDN(subject);
            certGenerator.SetSerialNumber(BigInteger.ValueOf(1));
            certGenerator.SetNotAfter(DateTime.UtcNow.AddHours(1));
            certGenerator.SetNotBefore(DateTime.UtcNow);
            certGenerator.SetPublicKey(subjectPublic);
            return certGenerator.Generate(signatureFactory);
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
