using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.Core;
using Lykke.Service.PayAuth.Core.Exceptions;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Models;
using LykkePay.Common.Validation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class PayAuthController : Controller
    {
        private readonly IPayAuthService _payAuthService;
        private readonly ILog _log;

        public PayAuthController(
            [NotNull] IPayAuthService payAuthService,
            [NotNull] ILogFactory logFactory)
        {
            _payAuthService = payAuthService ?? throw new ArgumentNullException(nameof(payAuthService));
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Gets pay auth information
        /// </summary>
        /// <param name="merchantId">Merchant id</param>
        [HttpGet]
        [SwaggerOperation(nameof(GetPayAuthInformation))]
        [ValidateModel]
        [ProducesResponseType(typeof(PayAuthInformationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPayAuthInformation([Required][RowKey] string merchantId)
        {
            try
            {
                var payAuth = await _payAuthService.GetAsync(merchantId, LykkePayConstants.DefaultSystemId);

                return Ok(Mapper.Map<PayAuthInformationResponse>(payAuth));
            }
            catch (ClientNotFoundException e)
            {
                _log.Error(e, $"{e.Message}, request: {merchantId}");

                return NotFound(ErrorResponse.Create(e.Message));
            }
        }

        /// <summary>
        /// Updates api key
        /// </summary>
        /// <param name="request">Model to update api key for merchant</param>
        [HttpPut]
        [SwaggerOperation(nameof(UpdateApiKey))]
        [ValidateModel]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateApiKey([FromBody] UpdateApiKeyModel request)
        {
            try
            {
                await _payAuthService.UpdateAsync(Mapper.Map<Core.Domain.PayAuth>(request));

                return Ok();
            }
            catch (ClientNotFoundException e)
            {
                _log.Error(e, $"{e.Message}, request: {request.ToJson()}");

                return NotFound(ErrorResponse.Create(e.Message));
            }
        }
    }
}
