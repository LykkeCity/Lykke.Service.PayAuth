﻿using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.Core.Services;

namespace Lykke.Service.PayAuth.Services
{
    // NOTE: Sometimes, shutdown process should be expressed explicitly. 
    // If this is your case, use this class to manage shutdown.
    // For example, sometimes some state should be saved only after all incoming message processing and 
    // all periodical handler was stopped, and so on.
    
    public class ShutdownManager : IShutdownManager
    {
        private readonly ILog _log;

        public ShutdownManager(ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(this);
        }

        public async Task StopAsync()
        {
            // TODO: Implement your shutdown logic here. Good idea is to log every step

            await Task.CompletedTask;
        }
    }
}
