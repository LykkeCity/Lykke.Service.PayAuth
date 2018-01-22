using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PayAuth.Core.Domain
{
    public interface IPayAuth
    {
        string Id { get; }
        string SystemId { get; }
        string ClientId { get; }
        string Certificate { get; }
        string ApiKey { get; }
    }
}
