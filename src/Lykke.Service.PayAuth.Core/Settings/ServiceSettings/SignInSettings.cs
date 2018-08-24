using System;

namespace Lykke.Service.PayAuth.Core.Settings.ServiceSettings
{
    public class PayAuthSettings
    {
        public DbSettings Db { get; set; }
        public TimeSpan ResetPasswordAccessTokenExpiration { get; set; }
    }
}
