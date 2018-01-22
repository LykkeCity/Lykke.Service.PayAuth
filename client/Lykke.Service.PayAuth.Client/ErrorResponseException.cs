using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PayAuth.Client
{
    public class ErrorResponseException : Exception
    {
        public ErrorResponseException()
        {
        }

        public ErrorResponseException(string message)
            : base(message)
        {
        }

        public ErrorResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
