using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.PayAuth.Client
{
    public static class AutofacExtension
    {
        public static void RegisterPayAuthClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<PayAuthClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IPayAuthClient>()
                .SingleInstance();
        }

        public static void RegisterPayAuthClient(this ContainerBuilder builder, PayAuthServiceClientSettings settings, ILog log)
        {
            builder.RegisterPayAuthClient(settings?.ServiceUrl, log);
        }
    }
}
