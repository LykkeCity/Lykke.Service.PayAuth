using System;
using System.Runtime.Serialization;

namespace Lykke.Service.PayAuth.Core.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException()
        {
        }

        public ClientNotFoundException(string clientId) : base("Client not found")
        {
            ClientId = clientId;
        }

        public ClientNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string ClientId { get; set; }
    }
}
