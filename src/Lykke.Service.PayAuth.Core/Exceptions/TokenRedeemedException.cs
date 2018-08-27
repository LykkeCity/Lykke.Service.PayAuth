using System;
using System.Runtime.Serialization;

namespace Lykke.Service.PayAuth.Core.Exceptions
{
    public class TokenRedeemedException : Exception
    {
        public TokenRedeemedException()
        {
        }

        public TokenRedeemedException(string token) : base("Token already redeemed")
        {
            Token = token;
        }

        public TokenRedeemedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TokenRedeemedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Token { get; set; }
    }
}
