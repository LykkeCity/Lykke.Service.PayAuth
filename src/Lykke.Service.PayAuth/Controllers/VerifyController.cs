using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.Core;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Exceptions;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Filters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Lykke.Service.PayAuth.Models;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class VerifyController : Controller
    {
        private readonly ISecurityHelper _securityHelper;
        private readonly IPayAuthService _payAuthService;
        private readonly ILog _log;

        public VerifyController(
            [NotNull] ISecurityHelper securityHelper,
            [NotNull] IPayAuthService payAuthService,
            [NotNull] ILogFactory logFactory)
        {
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
            _payAuthService = payAuthService ?? throw new ArgumentNullException(nameof(payAuthService));
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Verifies signature against clientId provided
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signature")]
        [SwaggerOperation("VerifySignature")]
        [ValidateModel]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SignatureValidationResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> VerifySignature([FromBody] VerifySignatureModel request)
        {
            try
            {
                IPayAuth payAuth = await _payAuthService.GetAsync(request.ClientId, request.SystemId);

                var validationResult = _securityHelper.CheckRequest(request.Text, request.ClientId, request.Signature,
                    payAuth.Certificate, payAuth.ApiKey);

                return Ok(new SignatureValidationResponse {Description = validationResult.ToString(), ErrorType = validationResult});
            }
            catch (ClientNotFoundException e)
            {
                _log.Error(e, $"{e.Message}, request: {request.ToJson()}");

                return NotFound(ErrorResponse.Create(e.Message));
            }
        }
    }
}
