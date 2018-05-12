using Lykke.Service.PayAuth.Core.Settings.ServiceSettings;
using Lykke.Service.PayAuth.Core.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayAuth.Core.Settings
{
    public class AppSettings
    {
        public PayAuthSettings PayAuthService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public MonitoringServiceClientSettings MonitoringServiceClient { get; set; }
    }

    public class MonitoringServiceClientSettings
    {
        [HttpCheck("api/isalive", false)]
        public string MonitoringServiceUrl { get; set; }
    }
}
