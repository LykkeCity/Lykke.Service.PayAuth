using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Repositories;
using Lykke.Service.PayAuth.Core.Services;

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
        
        public async Task RegisterAsync(string email, string password)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);
            
            if(credentials != null)
                throw new InvalidOperationException("Employee with same email already exists.");
            
            string salt = Guid.NewGuid().ToString();
            string hash = CalculateHash(password, salt);

            await _repository.InsertOrReplaceAsync(new EmployeeCredentials(email, hash, salt));

            await _log.WriteInfoAsync(nameof(EmployeeCredentialsService), nameof(RegisterAsync),
                new { Email = email.SanitizeEmail()}.ToString(),
                "Employee credentials registered.");
        }

        public async Task<bool> ValidateAsync(string email, string password)
        {
            IEmployeeCredentials credentials = await _repository.GetAsync(email);

            if (credentials == null)
                return false;
            
            string hash = CalculateHash(password, credentials.Salt);

            return credentials.Password.Equals(hash);
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
