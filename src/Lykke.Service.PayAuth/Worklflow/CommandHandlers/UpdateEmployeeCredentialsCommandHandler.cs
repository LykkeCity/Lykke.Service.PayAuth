using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Service.PayAuth.Contract.Commands;
using Lykke.Service.PayAuth.Contract.Events;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Services;

namespace Lykke.Service.PayAuth.Worklflow.CommandHandlers
{
    [UsedImplicitly]
    public class UpdateEmployeeCredentialsCommandHandler
    {
        private readonly IEmployeeCredentialsService _employeeCredentialsService;
        private readonly IChaosKitty _chaosKitty;
        private readonly ILog _log;

        public UpdateEmployeeCredentialsCommandHandler(
            [NotNull] IChaosKitty chaosKitty, 
            [NotNull] IEmployeeCredentialsService employeeCredentialsService,
            [NotNull] ILogFactory logFactory)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _employeeCredentialsService = employeeCredentialsService ?? throw new ArgumentNullException(nameof(employeeCredentialsService));
            _log = logFactory.CreateLog(this);
        }

        [UsedImplicitly]
        public async Task<CommandHandlingResult> Handle(UpdateEmployeeCredentialsCommand cmd, IEventPublisher publisher)
        {
            _log.Info("Handling UpdateEmployeeCredentialsCommand", $"Command details: {cmd.ToJson()}");

            if (string.IsNullOrEmpty(cmd.Password))
            {
                publisher.PublishEvent(new EmployeeUpdateCompletedEvent
                {
                    Id = cmd.EmployeeId
                });

                _chaosKitty.Meow("Issue with RabbitMq publishing EmployeeUpdateCompletedEvent");

                return CommandHandlingResult.Ok();
            }

            _log.Info("Going to update empoyee credetials");

            await _employeeCredentialsService.UpdateAsync(new EmployeeCredentials
            {
                EmployeeId = cmd.EmployeeId,
                Email = cmd.Email,
                MerchantId = cmd.MerchantId,
                Password = cmd.Password,
            });

            _log.Info("Going to publish EmployeeCredentialsUpdatedEvent");

            publisher.PublishEvent(new EmployeeCredentialsUpdatedEvent
            {
                EmployeeId = cmd.EmployeeId,
                MerchantId = cmd.MerchantId
            });

            _chaosKitty.Meow("Issue with RabbitMq publishing EmployeeCredentialsUpdatedEvent");

            return CommandHandlingResult.Ok();
        }
    }
}
