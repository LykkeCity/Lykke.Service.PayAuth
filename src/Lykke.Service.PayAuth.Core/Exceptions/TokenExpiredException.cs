using System;
using System.Runtime.Serialization;

namespace Lykke.Service.PayAuth.Core.Exceptions
{
    public class TokenExpiredException : Exception
    {
        public TokenExpiredException()
        {
        }

        public TokenExpiredException(string token) : base("Token expired")
        {
            Token = token;
        }

        public TokenExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TokenExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Token { get; set; }
    }
}
