using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IPayAuthService _payAuthService;
        public RegisterController(IPayAuthService payAuthService)
        {
            _payAuthService = payAuthService;
        }
        [HttpPost]
        [SwaggerOperation("Register")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(string systemId, string clientId, string apiKey, string certificate)
        {
            try
            {
                var model = new NewPayAuthModel
                {
                    ClientId = clientId,
                    SystemId = systemId,
                    ApiKey = apiKey,
                    Certificate = certificate
                };
                var sign = Mapper.Map<Core.Domain.PayAuth>(model);
                await _payAuthService.AddAsync(sign);
            }
            catch (Exception exception)
            {
                return BadRequest(ErrorResponse.Create(exception.Message));
            }
            return NoContent();
        }
    }
}
