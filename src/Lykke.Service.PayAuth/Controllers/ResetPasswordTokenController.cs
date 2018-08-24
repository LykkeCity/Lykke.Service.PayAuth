using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Exceptions;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Models;
using LykkePay.Common.Validation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class ResetPasswordTokenController : Controller
    {
        private readonly IResetPasswordAccessTokenService _accessTokenService;
        private readonly ILog _log;

        public ResetPasswordTokenController(
            [NotNull] IResetPasswordAccessTokenService accessTokenService,
            [NotNull] ILogFactory logFactory)
        {
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Creates new reset password token for the employee
        /// </summary>
        /// <param name="request">Token creations details</param>
        /// <response code="200">Token details</response>
        /// <response code="400">Token already exists</response>
        [HttpPost]
        [SwaggerOperation(nameof(Create))]
        [ProducesResponseType(typeof(ResetPasswordAccessTokenResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateResetPasswordTokenRequest request)
        {
            try
            {
                ResetPasswordAccessToken token =
                    await _accessTokenService.CreateAsync(request.EmployeeId, request.MerchantId);

                return Ok(Mapper.Map<ResetPasswordAccessTokenResponse>(token));
            }
            catch (DuplicateKeyException e)
            {
                _log.Error(e, $"{e.Message}, request: {request.ToJson()}");

                return BadRequest(ErrorResponse.Create(e.Message));
            }
        }

        /// <summary>
        /// Returns reset password access token
        /// </summary>
        /// <param name="publicId">Token public id</param>
        /// <response code="200">Token details</response>
        /// <response code="404">Token not found</response>
        [HttpGet]
        [Route("{publicId}")]
        [SwaggerOperation(nameof(GetByPublicId))]
        [ProducesResponseType(typeof(ResetPasswordAccessTokenResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByPublicId(string publicId)
        {
            ResetPasswordAccessToken token = await _accessTokenService.GetByPublicIdAsync(publicId);

            if (token == null)
                return NotFound(ErrorResponse.Create("Token not found"));

            return Ok(Mapper.Map<ResetPasswordAccessTokenResponse>(token));
        }

        /// <summary>
        /// Redeems reset password token
        /// </summary>
        /// <param name="publicId">Token public id</param>
        /// <response code="200">Token details</response>
        /// <response code="400">Tokden already redeemed or expired</response>
        /// <response code="404">Token not found</response>
        [HttpPut]
        [Route("{publicId}")]
        [SwaggerOperation(nameof(Redeem))]
        [ProducesResponseType(typeof(ResetPasswordAccessTokenResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Redeem(string publicId)
        {
            try
            {
                ResetPasswordAccessToken token = await _accessTokenService.RedeemAsync(publicId);

                return Ok(Mapper.Map<ResetPasswordAccessTokenResponse>(token));
            }
            catch (TokenNotFoundException e)
            {
                _log.Error(e, $"Token not found: {publicId}");

                return NotFound(ErrorResponse.Create(e.Message));
            }
            catch (TokenExpiredException e)
            {
                _log.Error(e, $"Token expired: {publicId}");

                return BadRequest(ErrorResponse.Create(e.Message));
            }
            catch (TokenRedeemedException e)
            {
                _log.Error(e, $"Token already redeemed: {publicId}");

                return BadRequest(ErrorResponse.Create(e.Message));
            }
        }
    }
}
