using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayAuth.Core.Domain;
using Lykke.Service.PayAuth.Core.Services;
using Lykke.Service.PayAuth.Core.Utils;
using Lykke.Service.PayAuth.Extensions;
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
            IEmployeeCredentialsService employeeCredentialsService,
            ILog log)
        {
            _employeeCredentialsService = employeeCredentialsService;
            _log = log;
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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse().AddErrors(ModelState));

            try
            {
                var credentials = Mapper.Map<EmployeeCredentials>(model);
                
                await _employeeCredentialsService.RegisterAsync(credentials);
            }
            catch (InvalidOperationException exception)
            {
                await _log.WriteWarningAsync(nameof(EmployeesController), nameof(RegisterAsync),
                    model.MerchantId
                        .ToContext(nameof(model.MerchantId))
                        .ToContext(nameof(model.EmployeeId), model.EmployeeId)
                        .ToContext(nameof(model.Email), model.Email.SanitizeEmail())
                        .ToJson(),
                    exception.Message);
                
                return BadRequest(ErrorResponse.Create(exception.Message));
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
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCredentialsModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse().AddErrors(ModelState));

            try
            {
                var credentials = Mapper.Map<EmployeeCredentials>(model);

                await _employeeCredentialsService.UpdateAsync(credentials);
            }
            catch (InvalidOperationException exception)
            {
                await _log.WriteWarningAsync(nameof(EmployeesController), nameof(UpdateAsync),
                    model.MerchantId
                        .ToContext(nameof(model.MerchantId))
                        .ToContext(nameof(model.EmployeeId), model.EmployeeId)
                        .ToContext(nameof(model.Email), model.Email.SanitizeEmail())
                        .ToJson(),
                    exception.Message);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Validates employee credentials.
        /// </summary>
        /// <param name="model">The employee credentials.</param>
        /// <response code="200">The employee credentias validation result.</response>
        /// <response code="400">Invalid model.</response>
        [HttpGet]
        [SwaggerOperation("EmployeesValidate")]
        [ProducesResponseType(typeof(ValidateResultModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ValidateAsync([FromQuery] ValidationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse().AddErrors(ModelState));

            IEmployeeCredentials employeeCredentials =
                await _employeeCredentialsService.ValidateAsync(model.Email, model.Password);

            if(employeeCredentials == null)
                return Ok(new ValidateResultModel(false));
            
            return Ok(new ValidateResultModel(true)
            {
                MerchantId = employeeCredentials.MerchantId,
                EmployeeId = employeeCredentials.EmployeeId
            });
        }
    }
}
