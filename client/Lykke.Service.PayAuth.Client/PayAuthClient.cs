using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PayAuth.Client.Models;
using Lykke.Service.PayAuth.Client.Api;
using System.Net.Http;
using Microsoft.Extensions.PlatformAbstractions;
using Refit;

namespace Lykke.Service.PayAuth.Client
{
    public class PayAuthClient : IPayAuthClient, IDisposable
    {
        private readonly ILog _log;
        private readonly IPayAuthApi _payAuthApi;
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
            _runner = new ApiRunner();
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            await _runner.RunAsync(() => _payAuthApi.RegisterAsync(request));
        }

        public async Task<string> VerifyAsync(VerifyRequest request)
        {
            return await _runner.RunAsync(() => _payAuthApi.VerifyAsync(request));
        }
    }
}
