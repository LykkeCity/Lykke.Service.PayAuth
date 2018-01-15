using Lykke.Service.PayAuth.Core.Settings.ServiceSettings;
using Lykke.Service.PayAuth.Core.Settings.SlackNotifications;

namespace Lykke.Service.PayAuth.Core.Settings
{
    public class AppSettings
    {
        public PayAuthSettings PayAuthService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
