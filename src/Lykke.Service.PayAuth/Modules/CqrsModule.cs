using System.Collections.Generic;
using Autofac;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Service.PayAuth.Contract;
using Lykke.Service.PayAuth.Contract.Commands;
using Lykke.Service.PayAuth.Contract.Events;
using Lykke.Service.PayAuth.Core.Settings.ServiceSettings;
using Lykke.Service.PayAuth.Worklflow.CommandHandlers;
using Lykke.Service.PayAuth.Worklflow.Sagas;
using Lykke.Service.PayInvoice.Contract.Events;
using Lykke.SettingsReader;

namespace Lykke.Service.PayAuth.Modules
{
    public class CqrsModule : Module
    {
        private readonly IReloadingManager<CqrsSettings> _settings;
        private readonly string _resetPasswordUrlTemplate;

        private static readonly string SelfContext = EmployeeCredentialsRegistrationBoundedContext.Name;

        public CqrsModule(IReloadingManager<CqrsSettings> settings, string resetPasswordUrlTemplate)
        {
            _settings = settings;
            _resetPasswordUrlTemplate = resetPasswordUrlTemplate;
        }

        protected override void Load(ContainerBuilder builder)
        {
            const string defaultRoute = "self";

            RegisterChaosKitty(builder);

            builder.Register(context => new AutofacDependencyResolver(context))
                .As<IDependencyResolver>()
                .SingleInstance();

            var rabbitSettings = new RabbitMQ.Client.ConnectionFactory
                {Uri = _settings.CurrentValue.RabbitMqConnectionString};

            builder.RegisterType<RegisterEmployeeCredentialsSaga>();

            builder.RegisterType<RegisterEmployeeCredentialsCommandHandler>();

            builder.RegisterType<GeneratePasswordResetTokenCommandHandler>()
                .WithParameter(TypedParameter.From(_resetPasswordUrlTemplate));

            builder.RegisterType<UpdateEmployeeCredentialsCommandHandler>();

            builder.Register(ctx =>
            {
                var logFactory = ctx.Resolve<ILogFactory>();
                return new MessagingEngine(
                    logFactory,
                    new TransportResolver(new Dictionary<string, TransportInfo>
                    {
                        {
                            "RabbitMq",
                            new TransportInfo(
                                rabbitSettings.Endpoint.ToString(),
                                rabbitSettings.UserName,
                                rabbitSettings.Password,
                                "None", "RabbitMq")
                        }
                    }),
                    new RabbitMqTransportFactory(logFactory));
            });

            builder.Register(ctx => new CqrsEngine(
                    ctx.Resolve<ILogFactory>(),
                    ctx.Resolve<IDependencyResolver>(),
                    ctx.Resolve<MessagingEngine>(),
                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                        "RabbitMq",
                        Messaging.Serialization.SerializationFormat.ProtoBuf,
                        environment: "lykke")),

                    Register.Saga<RegisterEmployeeCredentialsSaga>($"{SelfContext}.saga")
                        .ListeningEvents(
                            typeof(EmployeeRegisteredEvent), 
                            typeof(EmployeeUpdatedEvent))
                        .From("lykkepay-employee-registration")
                        .On("payauth")
                        .ListeningEvents(
                            typeof(EmployeeCredentialsRegisteredEvent),
                            typeof(EmployeeCredentialsUpdatedEvent))
                        .From(SelfContext)
                        .On(defaultRoute)
                        .PublishingCommands(
                            typeof(RegisterEmployeeCredentialsCommand),
                            typeof(UpdateEmployeeCredentialsCommand),
                            typeof(GeneratePasswordResetTokenCommand))
                        .To(SelfContext)
                        .With("commands"),

                    Register.BoundedContext(SelfContext)
                        .ListeningCommands(typeof(RegisterEmployeeCredentialsCommand))
                        .On(defaultRoute)
                        .WithCommandsHandler<RegisterEmployeeCredentialsCommandHandler>()
                        .PublishingEvents(typeof(EmployeeCredentialsRegisteredEvent))
                        .With("events")

                        .ListeningCommands(typeof(GeneratePasswordResetTokenCommand))
                        .On(defaultRoute)
                        .WithCommandsHandler<GeneratePasswordResetTokenCommandHandler>()
                        .PublishingEvents(
                            typeof(EmployeeRegistrationCompletedEvent),
                            typeof(EmployeeUpdateCompletedEvent))
                        .With("events")

                        .ListeningCommands(typeof(UpdateEmployeeCredentialsCommand))
                        .On(defaultRoute)
                        .WithCommandsHandler<UpdateEmployeeCredentialsCommandHandler>()
                        .PublishingEvents(
                            typeof(EmployeeUpdateCompletedEvent), 
                            typeof(EmployeeCredentialsUpdatedEvent))
                        .With("events")))
                .As<ICqrsEngine>()
                .SingleInstance()
                .AutoActivate();
        }

        private void RegisterChaosKitty(ContainerBuilder builder)
        {
            if (_settings.CurrentValue.ChaosKitty != null)
            {
                builder.RegisterType<ChaosKitty>()
                    .WithParameter(TypedParameter.From(_settings.CurrentValue.ChaosKitty.StateOfChaos))
                    .As<IChaosKitty>()
                    .SingleInstance();
            }
            else
            {
                builder.RegisterType<SilentChaosKitty>()
                    .As<IChaosKitty>()
                    .SingleInstance();
            }
        }
    }
}
