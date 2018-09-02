using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
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

        public UpdateEmployeeCredentialsCommandHandler(
            [NotNull] IChaosKitty chaosKitty, 
            [NotNull] IEmployeeCredentialsService employeeCredentialsService)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _employeeCredentialsService = employeeCredentialsService ?? throw new ArgumentNullException(nameof(employeeCredentialsService));
        }

        [UsedImplicitly]
        public async Task<CommandHandlingResult> Handle(UpdateEmployeeCredentialsCommand cmd, IEventPublisher publisher)
        {
            if (string.IsNullOrEmpty(cmd.Password))
            {
                publisher.PublishEvent(new EmployeeUpdateCompletedEvent
                {
                    Id = cmd.EmployeeId
                });

                _chaosKitty.Meow("Issue with RabbitMq publishing EmployeeUpdateCompletedEvent");

                return CommandHandlingResult.Ok();
            }

            await _employeeCredentialsService.UpdateAsync(new EmployeeCredentials
            {
                EmployeeId = cmd.EmployeeId,
                Email = cmd.Email,
                MerchantId = cmd.MerchantId,
                Password = cmd.Password,
            });

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
