using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Repositories;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Core.Utils;

namespace Lykke.Service.PayAuth.Services
{
    public class EmployeeCredentialsService : IEmployeeCredentialsService
    {
        private readonly IEmployeeCredentialsRepository _repository;
        private readonly ILog _log;

        public EmployeeCredentialsService(IEmployeeCredentialsRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }
        
        public async Task RegisterAsync(IEmployeeCredentials employeeCredentials)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(employeeCredentials.Email);
            
            if(credentials != null)
                throw new InvalidOperationException("Employee with same email already exists.");
            
            string salt = Guid.NewGuid().ToString();
            string hash = CalculateHash(employeeCredentials.Password, salt);

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials
            {
                MerchantId = employeeCredentials.MerchantId,
                EmployeeId = employeeCredentials.EmployeeId,
                Email = employeeCredentials.Email,
                Password = hash,
                Salt = salt
            });

            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(RegisterAsync),
                employeeCredentials.MerchantId
                    .ToContext(nameof(employeeCredentials.MerchantId))
                    .ToContext(nameof(employeeCredentials.EmployeeId), employeeCredentials.EmployeeId)
                    .ToContext(nameof(employeeCredentials.Email), employeeCredentials.Email.SanitizeEmail())
                    .ToJson(),
                "Employee credentials registered.");
        }
        public async Task UpdateAsync(IEmployeeCredentials employeeCredentials)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(employeeCredentials.Email);
            if (credentials != null)
            {
                string salt = Guid.NewGuid().ToString();
                string hash = CalculateHash(employeeCredentials.Password, salt);

                await _repository.InsertOrReplaceAsync(new EmployeeCredentials
                {
                    MerchantId = employeeCredentials.MerchantId,
                    EmployeeId = employeeCredentials.EmployeeId,
                    Email = employeeCredentials.Email,
                    Password = hash,
                    Salt = salt
                });

                await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(UpdateAsync),
                    employeeCredentials.MerchantId
                        .ToContext(nameof(employeeCredentials.MerchantId))
                        .ToContext(nameof(employeeCredentials.EmployeeId), employeeCredentials.EmployeeId)
                        .ToContext(nameof(employeeCredentials.Email), employeeCredentials.Email.SanitizeEmail())
                        .ToJson(),
                    "Employee credentials updated.");
            }
        }

        public async Task<IEmployeeCredentials> ValidateAsync(string email, string password)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                return null;
            
            string hash = CalculateHash(password, credentials.Salt);

            if (credentials.Password.Equals(hash))
                return credentials;

            return null;
        }

        public async Task DeleteAsync(string email)
        {
            await _repository.DeleteAsync(email);
            
            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(RegisterAsync),
                new { Email = email.SanitizeEmail()}.ToString(),
                "Employee credentials deleted.");
        }
        
        private static string CalculateHash(string password, string salt)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash($"{password}{salt}".ToUtf8Bytes()));
        }
    }
}
