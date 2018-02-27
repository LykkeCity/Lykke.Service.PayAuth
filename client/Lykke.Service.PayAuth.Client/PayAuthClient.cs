using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PayAuth.Client.Models;
using Lykke.Service.PayAuth.Client.Api;
using System.Net.Http;
using Lykke.Service.PayAuth.Client.Models.Employees;
using Microsoft.Extensions.PlatformAbstractions;
using Refit;

namespace Lykke.Service.PayAuth.Client
{
    public class PayAuthClient : IPayAuthClient, IDisposable
    {
        private readonly ILog _log;
        private readonly IPayAuthApi _payAuthApi;
        private readonly IEmployeesApi _employeesApi;
        private readonly ApiRunner _runner;
        private readonly HttpClient _httpClient;

        public PayAuthClient(PayAuthServiceClientSettings settings, ILog log)
        {
            _log = log;
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrEmpty(settings.ServiceUrl))
                throw new Exception("Service URL required");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(settings.ServiceUrl),
                DefaultRequestHeaders =
                {
                    {
                        "User-Agent",
                        $"{PlatformServices.Default.Application.ApplicationName}/{PlatformServices.Default.Application.ApplicationVersion}"
                    }
                }
            };

            _payAuthApi = RestService.For<IPayAuthApi>(_httpClient);
            _employeesApi = RestService.For<IEmployeesApi>(_httpClient);
            _runner = new ApiRunner();
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            await _runner.RunAsync(() => _payAuthApi.RegisterAsync(request));
        }

        public async Task<SignatureValidationResponse> VerifyAsync(VerifyRequest request)
        {
            return await _runner.RunAsync(() => _payAuthApi.VerifyAsync(request));
        }

        /// <summary>
        /// Registers an employee credentials.
        /// </summary>
        /// <param name="model">The registration details.</param>
        public async Task RegisterAsync(RegisterModel model)
        {
            await _runner.RunAsync(() => _employeesApi.RegisterAsync(model));
        }
        /// <summary>
        /// Update an employee credentials
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(RegisterModel model)
        {
            await _runner.RunAsync(() => _employeesApi.UpdateAsync(model));
        }
        /// <summary>
        /// Validates employee credentials.
        /// </summary>
        /// <param name="email">The employee email.</param>
        /// <param name="password">The employee password.</param>
        /// <returns>The validation result.</returns>
        public async Task<ValidateResultModel> ValidateAsync(string email, string password)
        {
            return await _runner.RunAsync(() => _employeesApi.ValidateAsync(email, password));
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
