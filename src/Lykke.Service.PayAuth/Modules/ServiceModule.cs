using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.AzureRepositories;
using Lykke.Service.PayAuth.AzureRepositories.EmployeeCredentials;
using Lykke.Service.PayAuth.AzureRepositories.ResetPasswordAccessToken;
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
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<PayAuthSettings> settings)
        {
            _settings = settings;
            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
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

            builder.RegisterType<ResetPasswordAccessTokenService>()
                .As<IResetPasswordAccessTokenService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.ResetPasswordAccessTokenExpiration));

            RegisterRepositories(builder);

            builder.Populate(_services);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            const string payauthTableName = "PayAuthSignature";
            const string employeeCredentialsTableName = "EmployeeCredentials";
            const string resetPasswordAccessTokenTableName = "ResetPasswordAccessToken";

            builder.Register(c =>
                    new PayAuthRepository(AzureTableStorage<PayAuthEntity>.Create(
                        _settings.ConnectionString(x => x.Db.DataConnString), payauthTableName,
                        c.Resolve<ILogFactory>())))
                .As<IPayAuthRepository>()
                .SingleInstance();

            builder.Register(c => new EmployeeCredentialsRepository(
                    AzureTableStorage<EmployeeCredentialsEntity>.Create(
                        _settings.ConnectionString(x => x.Db.DataConnString),
                        employeeCredentialsTableName, c.Resolve<ILogFactory>())))
                .As<IEmployeeCredentialsRepository>()
                .SingleInstance();

            builder.Register(c => new ResetPasswordAccessTokenRepository(
                    AzureTableStorage<ResetPasswordAccessTokenEntity>.Create(
                        _settings.ConnectionString(x => x.Db.DataConnString), resetPasswordAccessTokenTableName,
                        c.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                        resetPasswordAccessTokenTableName, c.Resolve<ILogFactory>())))
                .As<IResetPasswordAccessTokenRepository>()
                .SingleInstance();
        }
    }
}
