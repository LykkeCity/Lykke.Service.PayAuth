using System;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.SignIn.Controllers
{
    [Route("api/[controller]")]
    public class VerifyController : Controller
    {
        [HttpGet("signature")]
        [SwaggerOperation("VerifySignature")]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public Task<IActionResult> VerifySignature(string text, string signature, string systemId, string clientId)
        {
            throw new NotImplementedException();
        }
    }
}
