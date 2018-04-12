using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PayAuth.Client.Models;
using Lykke.Service.PayAuth.Client.Api;
using System.Net.Http;
using Lykke.Service.PayAuth.Client.Models.Employees;
using Microsoft.Extensions.PlatformAbstractions;
using Refit;
using System.Threading;

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

        public Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _runner.RunAsync(() => _payAuthApi.RegisterAsync(request, cancellationToken));
        }

        public Task<SignatureValidationResponse> VerifyAsync(VerifyRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _runner.RunAsync(() => _payAuthApi.VerifyAsync(request, cancellationToken));
        }

        public Task RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _runner.RunAsync(() => _employeesApi.RegisterAsync(model, cancellationToken));
        }

        public Task UpdateAsync(UpdateCredentialsModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _runner.RunAsync(() => _employeesApi.UpdateAsync(model, cancellationToken));
        }

        public Task<ValidateResultModel> ValidateAsync(string email, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _runner.RunAsync(() => _employeesApi.ValidateAsync(email, password, cancellationToken));
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
