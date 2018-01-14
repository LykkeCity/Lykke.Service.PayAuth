using Lykke.Service.SignIn.Core.Settings.ServiceSettings;
using Lykke.Service.SignIn.Core.Settings.SlackNotifications;

namespace Lykke.Service.SignIn.Core.Settings
{
    public class AppSettings
    {
        public SignInSettings SignInService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
