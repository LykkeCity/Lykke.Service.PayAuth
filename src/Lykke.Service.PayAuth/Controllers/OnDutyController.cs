using System;
using System.Net;
using JetBrains.Annotations;
using Lykke.Service.PayAuth.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PayAuth.Controllers
{
    [Route("api/[controller]")]
    public class OnDutyController : Controller
    {
        private readonly IEmployeeCredentialsService _employeeCredentialsService;

        public OnDutyController([NotNull] IEmployeeCredentialsService employeeCredentialsService)
        {
            _employeeCredentialsService = employeeCredentialsService ?? throw new ArgumentNullException(nameof(employeeCredentialsService));
        }

        /// <summary>
        /// Calculates SHA1 hash for string with salt
        /// </summary>
        /// <param name="source">String to calculate hash for</param>
        /// <param name="salt">Salt to be added to string before hash calculation</param>
        /// <returns>Base64 encoded string</returns>
        [HttpGet]
        [Route("hash/{source}/{salt}")]
        [SwaggerOperation(nameof(CalculateHash))]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        public IActionResult CalculateHash(string source, string salt)
        {
            return Ok(_employeeCredentialsService.CalculateHash(
                Uri.UnescapeDataString(source),
                Uri.UnescapeDataString(salt)));
        }
    }
}
