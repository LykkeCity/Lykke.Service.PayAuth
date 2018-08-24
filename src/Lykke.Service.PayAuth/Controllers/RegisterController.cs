using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Models;
using LykkePay.Common.Validation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IPayAuthService _payAuthService;
        public RegisterController(
            [NotNull] IPayAuthService payAuthService)
        {
            _payAuthService = payAuthService ?? throw new ArgumentNullException(nameof(payAuthService));
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
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] PayAuthModel request)
        {
            var newPayAuth = Mapper.Map<Core.Domain.PayAuth>(request);

            await _payAuthService.AddAsync(newPayAuth);

            return Ok();
        }
    }
}
