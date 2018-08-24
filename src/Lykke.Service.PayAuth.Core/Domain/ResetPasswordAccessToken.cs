using System;
using System.Security.Cryptography;
using System.Text;
using Common;

namespace Lykke.Service.PayAuth.Core.Domain
{
    public class ResetPasswordAccessToken
    {
        public string Id { get; set; }

        public string PublicId { get; set; }

        public string EmployeeId { get; set; }

        public string MerchantId { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool Redeemed { get; set; }

        public static ResetPasswordAccessToken Create(string employeeId, string merchantId, DateTime expiration)
        {
            string newId = StringUtils.GenerateId();

            return new ResetPasswordAccessToken
            {
                Id = newId,
                PublicId = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(newId))),
                EmployeeId = employeeId,
                MerchantId = merchantId,
                ExpiresOn = expiration,
                Redeemed = false
            };
        }
    }
}
