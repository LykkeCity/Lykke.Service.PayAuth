using System;
using Lykke.Common.Chaos;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayAuth.Core.Settings.ServiceSettings
{
    public class PayAuthSettings
    {
        public DbSettings Db { get; set; }
        public TimeSpan ResetPasswordAccessTokenExpiration { get; set; }
        public string ResetPasswordUrlTemplate { get; set; }
        public CqrsSettings Cqrs { get; set; }
    }

    public class CqrsSettings
    {
        [AmqpCheck]
        public string RabbitMqConnectionString { get; set; }

        [Optional]
        public ChaosSettings ChaosKitty { get; set; }
    }
}
