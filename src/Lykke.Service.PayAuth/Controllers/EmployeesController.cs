using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayAuth.Core.Services;
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
        [Route("")]
        [SwaggerOperation("EmployeesRegister")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse().AddErrors(ModelState));

            try
            {
                await _employeeCredentialsService.RegisterAsync(model.Email, model.Password);
            }
            catch (InvalidOperationException exception)
            {
                await _log.WriteWarningAsync(nameof(EmployeesController), nameof(RegisterAsync),
                    new {Email = model.Email.SanitizeEmail()}.ToString(),
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
        [Route("")]
        [SwaggerOperation("EmployeesValidate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ValidateAsync([FromQuery] ValidationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse().AddErrors(ModelState));
            
            bool result = await _employeeCredentialsService.ValidateAsync(model.Email, model.Password);

            return Ok(new ValidateResultModel(result));
        }
    }
}
