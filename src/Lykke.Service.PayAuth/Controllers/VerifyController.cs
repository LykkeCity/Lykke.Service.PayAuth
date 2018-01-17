using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Entities.Security;
using Lykke.Common.Extensions;
using Lykke.Service.PayAuth.Core;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class VerifyController : Controller
    {
        private readonly ISecurityHelper _securityHelper;
        private readonly IPayAuthService _payAuthService;
        public VerifyController(ISecurityHelper securityHelper, IPayAuthService payAuthService)
        {
            _securityHelper = securityHelper;
            _payAuthService = payAuthService;
        }
        [HttpGet("signature")]
        [SwaggerOperation("VerifySignature")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<SecurityErrorType> VerifySignature(string text, string signature, string systemId, string clientId)
        {
            var auth = await _payAuthService.GetAsync(clientId, systemId);
            if (auth != null)
            {
                return _securityHelper.CheckRequest(text, clientId, signature, auth.Certificate, auth.ApiKey);
            }
            return SecurityErrorType.SignIncorrect;
        }
    }
}
