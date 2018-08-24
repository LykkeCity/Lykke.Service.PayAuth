using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.Core;
using Lykke.Service.PayAuth.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Lykke.Service.PayAuth.Models;
using LykkePay.Common.Validation;
using Lykke.Service.PayAuth.Models.GenerateRsaKeys;
using JetBrains.Annotations;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class GenerateRsaKeysController : Controller
    {
        private readonly ISecurityHelper _securityHelper;
        private readonly IPayAuthService _payAuthService;
        private readonly ILog _log;

        public GenerateRsaKeysController(
            [NotNull] ISecurityHelper securityHelper,
            [NotNull] IPayAuthService payAuthService,
            [NotNull] ILogFactory logFactory)
        {
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
            _payAuthService = payAuthService ?? throw new ArgumentNullException(nameof(payAuthService));
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Generates rsa keys
        /// </summary>
        /// <param name="request">Model to generate rsa keys</param>
        [HttpPost]
        [SwaggerOperation(nameof(GenerateRsaKeys))]
        [ValidateModel]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SignatureValidationResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateRsaKeys([FromBody] GenerateRsaKeysModel request)
        {
            try
            {
                // check that pay auth for merchant exists
                var payAuth = await _payAuthService.GetAsync(request.ClientId, LykkePayConstants.DefaultSystemId);

                var rsaKeys = _securityHelper.GenerateRsaKeys(request.ClientDisplayName);

                await _payAuthService.UpdateAsync(new Core.Domain.PayAuth
                {
                    SystemId = LykkePayConstants.DefaultSystemId,
                    ClientId = request.ClientId,
                    Certificate = rsaKeys.PublicKey
                });

                return Ok(new GenerateRsaKeysResponse
                {
                    PublicKey = rsaKeys.PublicKey,
                    PrivateKey = rsaKeys.PrivateKey
                });
            }
            catch (Exception e)
            {
                _log.Error(e, $"{e.Message}, request: {request.ToJson()}");

                return BadRequest(ErrorResponse.Create(e.Message));
            }
        }
    }
}
