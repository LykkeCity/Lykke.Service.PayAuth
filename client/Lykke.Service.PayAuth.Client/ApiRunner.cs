using Refit;
using System;
using System.Threading.Tasks;

namespace Lykke.Service.PayAuth.Client
{
    internal class ApiRunner
    {
        public async Task RunAsync(Func<Task> method)
        {
            try
            {
                await method();
            }
            catch (ApiException exception)
            {
                throw new ErrorResponseException("An error occurred  during calling api", exception);
            }
        }

        public async Task<T> RunAsync<T>(Func<Task<T>> method)
        {
            try
            {
                return await method();
            }
            catch (ApiException exception)
            {
                throw new ErrorResponseException("An error occurred  during calling api", exception);
            }
        }
    }
}
