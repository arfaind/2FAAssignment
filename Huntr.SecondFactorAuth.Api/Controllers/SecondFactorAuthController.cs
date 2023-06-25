using Huntr.SecondFactorAuth.Contracts.Exceptions;
using Huntr.SecondFactorAuth.Contracts.Interfaces;
using Huntr.SecondFactorAuth.Contracts.Request;
using Huntr.SecondFactorAuth.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Api.Controllers
{
    [Route("api/auth/second-factor")]
    [ApiController]
    public class SecondFactorAuthController : ControllerBase
    {
        private readonly ILogger<SecondFactorAuthController> _logger;
        private readonly ICodeService _codeService;

        public SecondFactorAuthController(ILogger<SecondFactorAuthController> logger, ICodeService codeService)
        {
            _logger = logger;
            _codeService = codeService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendConfirmationCode([FromBody] SendCodeRequest sendCodeRequest)
        {
            try
            {
                var result = await _codeService.SendCodeAsync(sendCodeRequest);
                return Ok(result);
            }
            catch (TooManyCodesException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new ErrorInfo
                {
                    Message = ex.Message,
                    Code = "TooManyCode"
                });
            }
        }

        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> VerifyConfirmationCode([FromBody] VerifyCodeRequest verifyCodeRequest)
        {
            var result = await _codeService.VerifyCodeAsync(verifyCodeRequest);
            return Ok(result);
        }
    }
}
