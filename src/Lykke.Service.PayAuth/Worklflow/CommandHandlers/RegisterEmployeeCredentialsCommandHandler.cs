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
    public class RegisterEmployeeCredentialsCommandHandler
    {
        private readonly IEmployeeCredentialsService _employeeCredentialsService;
        private readonly IChaosKitty _chaosKitty;
        private readonly ILog _log;

        public RegisterEmployeeCredentialsCommandHandler(
            [NotNull] IChaosKitty chaosKitty, 
            [NotNull] IEmployeeCredentialsService employeeCredentialsService,
            [NotNull] ILogFactory logFactory)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _employeeCredentialsService = employeeCredentialsService ?? throw new ArgumentNullException(nameof(employeeCredentialsService));
            _log = logFactory.CreateLog(this);
        }

        [UsedImplicitly]
        public async Task<CommandHandlingResult> Handle(RegisterEmployeeCredentialsCommand cmd,
            IEventPublisher publisher)
        {
            try
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
            }
            catch (InvalidOperationException e)
            {
                // employee already exists
                _log.Warning(e.Message, context: $"Command details: {cmd.ToJson()}");
            }

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
