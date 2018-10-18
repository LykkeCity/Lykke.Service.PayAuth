using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
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

        public EmployeeCredentialsService(
            [NotNull] IEmployeeCredentialsRepository repository, 
            [NotNull] ILogFactory logFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _log = logFactory.CreateLog(this);
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
                ForcePasswordUpdate = employeeCredentials.ForcePasswordUpdate,
                ForcePinUpdate = employeeCredentials.ForcePinUpdate,
                ForceEmailConfirmation = employeeCredentials.ForcePinUpdate
            });

            _log.Info("Employee credentials registered.",
                employeeCredentials.MerchantId
                    .ToContext(nameof(employeeCredentials.MerchantId))
                    .ToContext(nameof(employeeCredentials.EmployeeId), employeeCredentials.EmployeeId)
                    .ToContext(nameof(employeeCredentials.Email), employeeCredentials.Email.SanitizeEmail()));
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
                ForcePasswordUpdate = false,
                ForcePinUpdate = false,
                ForceEmailConfirmation = credentials.ForceEmailConfirmation
            });

            _log.Info("Employee credentials updated.",
                employeeCredentials.MerchantId
                    .ToContext(nameof(employeeCredentials.MerchantId))
                    .ToContext(nameof(employeeCredentials.EmployeeId), employeeCredentials.EmployeeId)
                    .ToContext(nameof(employeeCredentials.Email), employeeCredentials.Email.SanitizeEmail()));
        }

        public async Task UpdatePasswordHashAsync(string email, string hash)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            var newCredentials = Mapper.Map<EmployeeCredentials>(credentials);

            newCredentials.Password = hash;
            newCredentials.ForcePasswordUpdate = false;

            await _repository.InsertOrReplaceAsync(newCredentials);

            _log.Info("Employee password updated.",
                credentials.MerchantId
                    .ToContext(nameof(credentials.MerchantId))
                    .ToContext(nameof(credentials.EmployeeId), credentials.EmployeeId)
                    .ToContext(nameof(credentials.Email), credentials.Email.SanitizeEmail())
                    .ToJson());
        }

        public async Task UpdatePinHashAsync(string email, string hash)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            var newCredentials = Mapper.Map<EmployeeCredentials>(credentials);

            newCredentials.ForcePinUpdate = false;
            newCredentials.PinCode = hash;

            await _repository.InsertOrReplaceAsync(newCredentials);

            _log.Info("Employee pin updated.",
                credentials.MerchantId
                    .ToContext(nameof(credentials.MerchantId))
                    .ToContext(nameof(credentials.EmployeeId), credentials.EmployeeId)
                    .ToContext(nameof(credentials.Email), credentials.Email.SanitizeEmail()));
        }

        public async Task EnforceCredentialsUpdateAsync(string email)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);
            
            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            var newCredentials = Mapper.Map<EmployeeCredentials>(credentials);

            newCredentials.ForcePasswordUpdate = true;
            newCredentials.ForcePinUpdate = true;

            await _repository.InsertOrReplaceAsync(newCredentials);

            _log.Info("Employee first time login flag set.",
                credentials.MerchantId
                    .ToContext(nameof(credentials.MerchantId))
                    .ToContext(nameof(credentials.EmployeeId), credentials.EmployeeId)
                    .ToContext(nameof(credentials.Email), credentials.Email.SanitizeEmail()));
        }

        public async Task SetEmailConfirmedAsync(string email)
        {
            var credentials = await _repository.UpdateEmailConfirmationAttributeAsync(email);

            if (credentials == null)
                throw new InvalidOperationException("Employee does not exist.");

            _log.Info("Employee email confirmed, details: " +
                      new {credentials.MerchantId, credentials.EmployeeId}.ToJson());
        }

        public async Task<IEmployeeCredentials> ValidatePasswordAsync(string email, string password)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (string.IsNullOrEmpty(credentials?.Password))
                return null;

            bool passed = credentials.Password.Equals(password) ||
                          credentials.Password.Equals(CalculateHash(password, credentials.Salt));

            return passed ? credentials : null;
        }

        public async Task<IEmployeeCredentials> ValidatePinAsync(string email, string pin)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (string.IsNullOrEmpty(credentials?.PinCode))
                return null;

            bool passed = credentials.PinCode.Equals(pin) ||
                          credentials.PinCode.Equals(CalculateHash(pin, credentials.Salt));

            return passed ? credentials : null;
        }

        public async Task DeleteAsync(string email)
        {
            await _repository.DeleteAsync(email);

            _log.Info("Employee credentials deleted.", new {Email = email.SanitizeEmail()});
        }

        string IEmployeeCredentialsService.CalculateHash(string source, string salt)
        {
            return CalculateHash(source, salt);
        }

        private static string CalculateHash(string password, string salt)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash($"{password}{salt}".ToUtf8Bytes()));
        }
    }
}
