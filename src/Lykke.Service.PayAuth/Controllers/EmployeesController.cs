using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Core.Utils;
using Lykke.Service.PayAuth.Filters;
using Lykke.Service.PayAuth.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeCredentialsService _employeeCredentialsService;
        private readonly ILog _log;

        public EmployeesController(
            [NotNull] IEmployeeCredentialsService employeeCredentialsService,
            [NotNull] ILogFactory logFactory)
        {
            _employeeCredentialsService = employeeCredentialsService ?? throw new ArgumentNullException(nameof(employeeCredentialsService));
            _log = logFactory.CreateLog(this);
        }
        
        /// <summary>
        /// Registers an employee credentials.
        /// </summary>
        /// <param name="model">The employee credentials.</param>
        /// <response code="204">The employee credentials successfully registered.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPost]
        [SwaggerOperation("EmployeesRegister")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            try
            {
                var credentials = Mapper.Map<EmployeeCredentials>(model);
                
                await _employeeCredentialsService.RegisterAsync(credentials);
            }
            catch (InvalidOperationException e)
            {
                _log.Warning(e.Message, e,
                    model.MerchantId
                        .ToContext(nameof(model.MerchantId))
                        .ToContext(nameof(model.EmployeeId), model.EmployeeId)
                        .ToContext(nameof(model.Email), model.Email.SanitizeEmail()));
                
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Updates an employee credentials.
        /// </summary>
        /// <param name="model">The employee credentials.</param>
        /// <response code="204">The employee credentials successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPut]
        [SwaggerOperation("EmployeesUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCredentialsModel model)
        {
            try
            {
                var credentials = Mapper.Map<EmployeeCredentials>(model);

                await _employeeCredentialsService.UpdateAsync(credentials);
            }
            catch (InvalidOperationException e)
            {
                _log.Warning(e.Message, e,
                    model.MerchantId
                        .ToContext(nameof(model.MerchantId))
                        .ToContext(nameof(model.EmployeeId), model.EmployeeId)
                        .ToContext(nameof(model.Email), model.Email.SanitizeEmail()));

                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Validates employee password.
        /// </summary>
        /// <param name="model">The employee password.</param>
        /// <response code="200">The employee password validation result.</response>
        /// <response code="400">Invalid model.</response>
        [HttpGet]
        [Route("password")]
        [SwaggerOperation(nameof(ValidatePassword))]
        [ProducesResponseType(typeof(CredentialsValidationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> ValidatePassword([FromQuery] PasswordValidationModel model)
        {
            IEmployeeCredentials employeeCredentials =
                await _employeeCredentialsService.ValidatePasswordAsync(model.Email, model.Password);

            if (employeeCredentials == null)
                return Ok(new CredentialsValidationResultModel(false));

            return Ok(new CredentialsValidationResultModel(true)
            {
                MerchantId = employeeCredentials.MerchantId,
                EmployeeId = employeeCredentials.EmployeeId,
                ForcePasswordUpdate = employeeCredentials.ForcePasswordUpdate,
                ForcePinUpdate = employeeCredentials.ForcePinUpdate
            });
        }

        /// <summary>
        /// Updates an employee password hash
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("password/hash")]
        [SwaggerOperation(nameof(UpdatePasswordHash))]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> UpdatePasswordHash([FromBody] UpdatePasswordHashModel model)
        {
            try
            {
                await _employeeCredentialsService.UpdatePasswordHashAsync(model.Email, model.PasswordHash);
            }
            catch (InvalidOperationException e)
            {
                _log.Warning(e.Message, e, model);

                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Marks employee password to be updated
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("credentials/forceUpdate")]
        [SwaggerOperation(nameof(EnforceCredentialsUpdate))]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> EnforceCredentialsUpdate([FromBody] EnforceCredentialsUpdateModel model)
        {
            try
            {
                await _employeeCredentialsService.EnforceCredentialsUpdateAsync(model.Email);
            }
            catch (InvalidOperationException e)
            {
                _log.Warning(e.Message, e, model);

                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Validates employee pin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pin")]
        [SwaggerOperation(nameof(ValidatePin))]
        [ProducesResponseType(typeof(CredentialsValidationResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> ValidatePin([FromQuery] PinValidationModel model)
        {
            IEmployeeCredentials employeeCredentials =
                await _employeeCredentialsService.ValidatePinAsync(model.Email, model.Pin);

            if (employeeCredentials == null)
                return Ok(new CredentialsValidationResultModel(false));

            return Ok(new CredentialsValidationResultModel(true)
            {
                MerchantId = employeeCredentials.MerchantId,
                EmployeeId = employeeCredentials.EmployeeId,
                ForcePasswordUpdate = employeeCredentials.ForcePasswordUpdate,
                ForcePinUpdate = employeeCredentials.ForcePinUpdate
            });
        }

        /// <summary>
        /// Updates an employee pin hash
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pin/hash")]
        [SwaggerOperation(nameof(UpdatePinHash))]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> UpdatePinHash([FromBody] UpdatePinHashModel model)
        {
            try
            {
                await _employeeCredentialsService.UpdatePinHashAsync(model.Email, model.PinHash);
            }
            catch (InvalidOperationException e)
            {
                _log.Warning(e.Message, e, model);

                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return NoContent();
        }
    }
}
