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
            
            string salt = employeeCredentials.Email;
            string hash = CalculateHash(employeeCredentials.Password, salt);

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials
            {
                MerchantId = employeeCredentials.MerchantId,
                EmployeeId = employeeCredentials.EmployeeId,
                Email = employeeCredentials.Email,
                Password = hash,
                Salt = salt,
                PinCode = null,
                UpdatePassword = employeeCredentials.UpdatePassword
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

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            string salt = employeeCredentials.Email;
            string hash = CalculateHash(employeeCredentials.Password, salt);

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials
            {
                MerchantId = employeeCredentials.MerchantId,
                EmployeeId = employeeCredentials.EmployeeId,
                Email = employeeCredentials.Email,
                Password = hash,
                Salt = salt,
                UpdatePassword = false
            });

            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(RegisterAsync),
                employeeCredentials.MerchantId
                    .ToContext(nameof(employeeCredentials.MerchantId))
                    .ToContext(nameof(employeeCredentials.EmployeeId), employeeCredentials.EmployeeId)
                    .ToContext(nameof(employeeCredentials.Email), employeeCredentials.Email.SanitizeEmail())
                    .ToJson(),
                "Employee credentials updated.");
        }

        public async Task UpdatePasswordHashAsync(string email, string hash)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials
            {
                MerchantId = credentials.MerchantId,
                EmployeeId = credentials.EmployeeId,
                Email = credentials.Email,
                PinCode = credentials.PinCode,
                Salt = credentials.Salt,
                Password = hash,
                UpdatePassword = false
            });

            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(UpdatePasswordHashAsync),
                credentials.MerchantId
                    .ToContext(nameof(credentials.MerchantId))
                    .ToContext(nameof(credentials.EmployeeId), credentials.EmployeeId)
                    .ToContext(nameof(credentials.Email), credentials.Email.SanitizeEmail())
                    .ToJson(),
                "Employee password updated.");
        }

        public async Task UpdatePinHashAsync(string email, string hash)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials
            {
                MerchantId = credentials.MerchantId,
                EmployeeId = credentials.EmployeeId,
                Email = credentials.Email,
                UpdatePassword = credentials.UpdatePassword,
                Password = credentials.Password,
                Salt = credentials.Salt,
                PinCode = hash
            });

            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(UpdatePinHashAsync),
                credentials.MerchantId
                    .ToContext(nameof(credentials.MerchantId))
                    .ToContext(nameof(credentials.EmployeeId), credentials.EmployeeId)
                    .ToContext(nameof(credentials.Email), credentials.Email.SanitizeEmail())
                    .ToJson(),
                "Employee pin updated.");
        }

        public async Task EnforcePasswordUpdateAsync(string email)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials
            {
                MerchantId = credentials.MerchantId,
                EmployeeId = credentials.EmployeeId,
                Email = credentials.Email,
                Password = credentials.Password,
                Salt = credentials.Salt,
                PinCode = credentials.PinCode,
                UpdatePassword = true
            });

            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(EnforcePasswordUpdateAsync),
                credentials.MerchantId
                    .ToContext(nameof(credentials.MerchantId))
                    .ToContext(nameof(credentials.EmployeeId), credentials.EmployeeId)
                    .ToContext(nameof(credentials.Email), credentials.Email.SanitizeEmail())
                    .ToJson(),
                "Employee first time login flag set.");
        }

        public async Task<IEmployeeCredentials> ValidatePasswordAsync(string email, string password)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                return null;

            bool passed = credentials.Password.Equals(password) ||
                          credentials.Password.Equals(CalculateHash(password, credentials.Salt));

            return passed ? credentials : null;
        }

        public async Task<IEmployeeCredentials> ValidatePinAsync(string email, string pin)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                return null;

            bool passed = credentials.PinCode.Equals(pin) ||
                          credentials.PinCode.Equals(CalculateHash(pin, credentials.Salt));

            return passed ? credentials : null;
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
