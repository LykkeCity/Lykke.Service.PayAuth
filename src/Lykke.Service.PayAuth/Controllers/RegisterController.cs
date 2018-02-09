using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Filters;
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

        /// <summary>
        /// Registers new client in the system
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("Register")]
        [ValidateModel]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] PayAuthModel request)
        {
            try
            {
                var newPayAuth = Mapper.Map<Core.Domain.PayAuth>(request);

                await _payAuthService.AddAsync(newPayAuth);

                return Ok();
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RegisterController), nameof(Register), request.ToJson(), ex);
            }

            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}
