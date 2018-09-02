using System.Collections.Generic;
using Autofac;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Service.PayAuth.Contract.Commands;
using Lykke.Service.PayAuth.Contract.Events;
using Lykke.Service.PayAuth.Core.Settings.ServiceSettings;
using Lykke.Service.PayAuth.Worklflow.CommandHandlers;
using Lykke.Service.PayAuth.Worklflow.Sagas;
using Lykke.Service.PayInvoice.Contract;
using Lykke.Service.PayInvoice.Contract.Events;
using Lykke.SettingsReader;

namespace Lykke.Service.PayAuth.Modules
{
    public class CqrsModule : Module
    {
        private readonly IReloadingManager<CqrsSettings> _settings;
        private readonly string _resetPasswordUrlTemplate;

        private const string CommandsRoute = "commands";
        private const string EventsRoute = "events";

        public CqrsModule(IReloadingManager<CqrsSettings> settings, string resetPasswordUrlTemplate)
        {
            _settings = settings;
            _resetPasswordUrlTemplate = resetPasswordUrlTemplate;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterChaosKitty(builder);

            builder.Register(context => new AutofacDependencyResolver(context))
                .As<IDependencyResolver>()
                .SingleInstance();

            var rabbitSettings = new RabbitMQ.Client.ConnectionFactory
                { Uri = _settings.CurrentValue.RabbitMqConnectionString };

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

                Register.Saga<RegisterEmployeeCredentialsSaga>(nameof(RegisterEmployeeCredentialsSaga))
                    .ListeningEvents(
                        typeof(EmployeeRegisteredEvent),
                        typeof(EmployeeUpdatedEvent),
                        typeof(EmployeeCredentialsRegisteredEvent),
                        typeof(EmployeeCredentialsUpdatedEvent))
                    .From(EmployeeRegistrationBoundedContext.Name)
                    .On(EventsRoute)
                    .PublishingCommands(
                        typeof(RegisterEmployeeCredentialsCommand),
                        typeof(UpdateEmployeeCredentialsCommand),
                        typeof(GeneratePasswordResetTokenCommand))
                    .To(EmployeeRegistrationBoundedContext.Name)
                    .With(CommandsRoute),

                Register.BoundedContext(EmployeeRegistrationBoundedContext.Name)
                    .ListeningCommands(typeof(RegisterEmployeeCredentialsCommand))
                    .On(CommandsRoute)
                    .WithCommandsHandler<RegisterEmployeeCredentialsCommandHandler>()
                    .PublishingEvents(typeof(EmployeeCredentialsRegisteredEvent))
                    .With(EventsRoute)

                    .ListeningCommands(typeof(GeneratePasswordResetTokenCommand))
                    .On(CommandsRoute)
                    .WithCommandsHandler<GeneratePasswordResetTokenCommandHandler>()
                    .PublishingEvents(typeof(EmployeeRegistrationCompletedEvent), typeof(EmployeeUpdateCompletedEvent))
                    .With(EventsRoute)

                    .ListeningCommands(typeof(UpdateEmployeeCredentialsCommand))
                    .On(CommandsRoute)
                    .WithCommandsHandler<UpdateEmployeeCredentialsCommandHandler>()
                    .PublishingEvents(typeof(EmployeeUpdateCompletedEvent), typeof(EmployeeCredentialsUpdatedEvent))
                    .With(EventsRoute)));
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
