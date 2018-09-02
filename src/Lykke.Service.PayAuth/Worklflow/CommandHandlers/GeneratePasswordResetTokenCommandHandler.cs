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
    public class GeneratePasswordResetTokenCommandHandler
    {
        private readonly IResetPasswordAccessTokenService _accessTokenService;
        private readonly IChaosKitty _chaosKitty;
        private readonly string _resetPasswordUrlTemplate;

        public GeneratePasswordResetTokenCommandHandler(
            [NotNull] IChaosKitty chaosKitty, 
            [NotNull] IResetPasswordAccessTokenService accessTokenService, 
            [NotNull] string resetPasswordUrlTemplate)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _resetPasswordUrlTemplate = resetPasswordUrlTemplate ?? throw new ArgumentNullException(nameof(resetPasswordUrlTemplate));
        }

        [UsedImplicitly]
        public async Task<CommandHandlingResult> Handle(GeneratePasswordResetTokenCommand cmd,
            IEventPublisher publisher)
        {
            ResetPasswordAccessToken token = await _accessTokenService.CreateAsync(cmd.EmployeeId, cmd.MerchantId);

            _chaosKitty.Meow("Issue with Azure repository when creating reset password token");

            string resetPasswordUrl = string.Format(_resetPasswordUrlTemplate, token.PublicId);

            if (cmd.IsNewEmployee)
            {
                publisher.PublishEvent(new EmployeeRegistrationCompletedEvent
                {
                    Id = token.EmployeeId,
                    ResetPasswordUrl = resetPasswordUrl
                });
            }
            else
            {
                publisher.PublishEvent(new EmployeeUpdateCompletedEvent
                {
                    Id = token.EmployeeId,
                    ResetPasswordUrl = resetPasswordUrl
                });
            }

            _chaosKitty.Meow("Issue with RabbitMq publishing EmployeeRegistrationCompletedEvent and EmployeeUpdateCompletedEvent");

            return CommandHandlingResult.Ok();
        }
    }
}
