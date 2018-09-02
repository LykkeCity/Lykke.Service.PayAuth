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
    public class RegisterEmployeeCredentialsCommandHandler
    {
        private readonly IEmployeeCredentialsService _employeeCredentialsService;
        private readonly IChaosKitty _chaosKitty;

        public RegisterEmployeeCredentialsCommandHandler(
            [NotNull] IChaosKitty chaosKitty, 
            [NotNull] IEmployeeCredentialsService employeeCredentialsService)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _employeeCredentialsService = employeeCredentialsService ?? throw new ArgumentNullException(nameof(employeeCredentialsService));
        }

        [UsedImplicitly]
        public async Task<CommandHandlingResult> Handle(RegisterEmployeeCredentialsCommand cmd,
            IEventPublisher publisher)
        {
            await _employeeCredentialsService.RegisterAsync(new EmployeeCredentials
            {
                EmployeeId = cmd.EmployeeId,
                MerchantId = cmd.MerchantId,
                Email = cmd.Email,
                Password = cmd.Password,
                ForcePasswordUpdate = true,
                ForcePinUpdate = true
            });

            publisher.PublishEvent(new EmployeeCredentialsRegisteredEvent
            {
                EmployeeId = cmd.EmployeeId,
                MerchantId = cmd.MerchantId
            });

            _chaosKitty.Meow("Issue with RabbitMq publishing EmployeeCredentialsRegisteredEvent");

            return CommandHandlingResult.Ok();
        }
    }
}
