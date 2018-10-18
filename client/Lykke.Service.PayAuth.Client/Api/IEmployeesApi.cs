using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.PayAuth.Client.Models.Employees;
using Refit;

namespace Lykke.Service.PayAuth.Client.Api
{
    internal interface IEmployeesApi
    {
        [Post("/api/employees")]
        Task RegisterAsync([Body] RegisterModel model, CancellationToken cancellationToken = default(CancellationToken));

        [Put("/api/employees")]
        Task UpdateAsync([Body] UpdateCredentialsModel model, CancellationToken cancellationToken = default(CancellationToken));

        [Get("/api/employees/password")]
        Task<ValidateResultModel> ValidatePasswordAsync(string email, string password);

        [Post("/api/employees/password/hash")]
        Task UpdatePasswordHashAsync([Body] UpdatePasswordHashModel model);

        [Post("/api/employees/credentials/forceUpdate")]
        Task EnforceCredentialsUpdateAsync([Body] EnforceCredentialsUpdateModel model);

        [Post("/api/employees/confirmEmail")]
        Task SetEmailConfirmedAsync([Body] EmailConfirmedRequest model);

        [Get("/api/employees/pin")]
        Task<ValidateResultModel> ValidatePinAsync(string email, string pin);

        [Post("/api/employees/pin/hash")]
        Task UpdatePinHashAsync([Body] UpdatePinHashModel model);
    }
}
