﻿
using Lykke.Service.PayAuth.Client.Models;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models.Employees;

namespace Lykke.Service.PayAuth.Client
{
    public interface IPayAuthClient
    {
        Task RegisterAsync(RegisterRequest request);
        Task<string> VerifyAsync(VerifyRequest request);

        /// <summary>
        /// Registers an employee credentials.
        /// </summary>
        /// <param name="model">The registration details.</param>
        Task RegisterAsync(RegisterModel model);

        /// <summary>
        /// Validates employee credentials.
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <param name="password">The employee password.</param>
        /// <returns>The validation result.</returns>
        Task<ValidateResultModel> ValidateAsync(string email, string password);
    }
}
