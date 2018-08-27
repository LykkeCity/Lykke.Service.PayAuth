using System;
using System.Runtime.Serialization;

namespace Lykke.Service.PayAuth.Core.Exceptions
{
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException()
        {
        }

        public TokenNotFoundException(string token) : base("Token not found")
        {
            Token = token;
        }

        public TokenNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TokenNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Token { get; set; }
    }
}
