using System;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Service.PayAuth.Contract;
using Lykke.Service.PayAuth.Contract.Commands;
using Lykke.Service.PayAuth.Contract.Events;
using Lykke.Service.PayInvoice.Contract.Events;

namespace Lykke.Service.PayAuth.Worklflow.Sagas
{
    [UsedImplicitly]
    public class RegisterEmployeeCredentialsSaga
    {
        private readonly IChaosKitty _chaosKitty;
        private readonly ILog _log;

        public RegisterEmployeeCredentialsSaga([NotNull] IChaosKitty chaosKitty, [NotNull] ILogFactory logFactory)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _log = logFactory.CreateLog(this);
        }

        [UsedImplicitly]
        private void Handle(EmployeeRegisteredEvent evt, ICommandSender sender)
        {
            sender.SendCommand(new RegisterEmployeeCredentialsCommand
            {
                Email = evt.Email,
                EmployeeId = evt.Id,
                MerchantId = evt.MerchantId,
                Password = evt.Password
            }, EmployeeCredentialsRegistrationBoundedContext.Name);

            _chaosKitty.Meow("Issue with RabbitMq when publishing RegisterEmployeeCredentialsCommand");
        }

        [UsedImplicitly]
        private void Handle(EmployeeCredentialsRegisteredEvent evt, ICommandSender sender)
        {
            sender.SendCommand(new GeneratePasswordResetTokenCommand
            {
                MerchantId = evt.MerchantId,
                EmployeeId = evt.EmployeeId,
                IsNewEmployee = true
            }, EmployeeCredentialsRegistrationBoundedContext.Name);

            _chaosKitty.Meow("Issue with RabbitMq when publishing GeneratePasswordResetTokenCommand");
        }

        [UsedImplicitly]
        private void Handle(EmployeeUpdatedEvent evt, ICommandSender sender)
        {
            _log.Info("Sending command UpdateEmployeeCredentialsCommand", $"Details: {evt.ToJson()}");

            sender.SendCommand(new UpdateEmployeeCredentialsCommand
            {
                Email = evt.Email,
                EmployeeId = evt.Id,
                MerchantId = evt.MerchantId,
                Password = evt.Password
            }, EmployeeCredentialsRegistrationBoundedContext.Name);

            _chaosKitty.Meow("Issue with RabbitMq when publishing UpdateEmployeeCredentialsCommand");
        }

        [UsedImplicitly]
        private void Handle(EmployeeCredentialsUpdatedEvent evt, ICommandSender sender)
        {
            sender.SendCommand(new GeneratePasswordResetTokenCommand
            {
                MerchantId = evt.MerchantId,
                EmployeeId = evt.EmployeeId,
                IsNewEmployee = false
            }, EmployeeCredentialsRegistrationBoundedContext.Name);

            _chaosKitty.Meow("Issue with RabbitMq when publishing GeneratePasswordResetTokenCommand");
        }
    }
}
