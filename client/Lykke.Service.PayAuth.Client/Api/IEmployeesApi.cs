using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models.Employees;
using Refit;

namespace Lykke.Service.PayAuth.Client.Api
{
    public interface IEmployeesApi
    {
        [Post("/api/employees")]
        Task RegisterAsync([Body] RegisterModel model);
        
        [Get("/api/employees")]
        Task<ValidateResultModel> ValidateAsync(string email, string password);
    }
}
