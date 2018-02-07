using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.PayAuth.AzureRepositories;
using Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials;
using Lykke.Service.PayAuth.Core;
using Lykke.Service.PayAuth.Core.Repositories;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Core.Settings.ServiceSettings;
using Lykke.Service.PayAuth.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.PayAuth.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<PayAuthSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<PayAuthSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<PayAuthService>()
                .As<IPayAuthService>();
            builder.RegisterType<SecurityHelper>()
                .As<ISecurityHelper>();

            builder.RegisterType<EmployeeCredentialsService>()
                .As<IEmployeeCredentialsService>();
            
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            // TODO: Add your dependencies here
            RegisterRepositories(builder);

            builder.Populate(_services);
        }
        private void RegisterRepositories(ContainerBuilder builder)
        {
            const string payauthTableName = "PayAuthSignature";
            const string employeeCredentialsTableName = "EmployeeCredentials";
            
            builder.Register(c => new PayAuthRepository(
                    AzureTableStorage<PayAuthEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                        payauthTableName, _log)))
                .As<IPayAuthRepository>();
            
            builder.Register(c => new EmployeeCredentialsRepository(
                    AzureTableStorage<EmployeeCredentialsEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                        employeeCredentialsTableName, _log)))
                .As<IEmployeeCredentialsRepository>();
        }
    }
}
