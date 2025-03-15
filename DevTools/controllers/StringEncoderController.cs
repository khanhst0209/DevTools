
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTools.controllers
{

    [ApiController]
    [Route("StringEncoder")]
    public class StringEncoderController : ControllerBase
    {
        private readonly IStringEncoderService _stringencoderservice;
        public StringEncoderController(IStringEncoderService _stringencoderservice)
        {
            this._stringencoderservice = _stringencoderservice;
        }

        [HttpGet("Base64")]
        [Authorize("user")]
        public async Task<IActionResult> GetBase64String(string s)
        {
            string result = _stringencoderservice.EncodeStringToBase64(s);
            return Ok(result);
        }
    }
}