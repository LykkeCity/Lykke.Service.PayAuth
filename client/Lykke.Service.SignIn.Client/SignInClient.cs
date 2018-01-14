using System;
using Common.Log;

namespace Lykke.Service.SignIn.Client
{
    public class SignInClient : ISignInClient, IDisposable
    {
        private readonly ILog _log;

        public SignInClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
