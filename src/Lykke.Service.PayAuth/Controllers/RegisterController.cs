using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Extensions;
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
        private readonly ILog _log;
        public RegisterController(IPayAuthService payAuthService, ILog log)
        {
            _payAuthService = payAuthService;
            _log = log;
        }
        [HttpPost("register")]
        [SwaggerOperation("Register")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody]PayAuthModel request)
        {
            var model = new NewPayAuthModel
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = request.ClientId,
                SystemId = request.SystemId,
                ApiKey = request.ApiKey,
                Certificate = request.Certificate
            };
            try
            {  
                var sign = Mapper.Map<Core.Domain.PayAuth>(model);
                await _payAuthService.AddAsync(sign);
            }
            catch (Exception exception)
            {
                await _log.WriteInfoAsync(nameof(RegisterController), nameof(Register), model.Id, exception.Message);
                return StatusCode(500);
            }
            return NoContent();
        }
    }
}
