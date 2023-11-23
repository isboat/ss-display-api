using Display.Models.App;
using Display.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceAuthenticationService _deviceService;

        public DeviceController(IDeviceAuthenticationService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost("code")]
        public async Task<IActionResult> GetDeviceCode(CodeRequest codeRequest)
        {
            var response = await _deviceService.GetDeviceCode(codeRequest);
            if (response == null)
            {
                return NotFound();
            }

            return new OkObjectResult(response);
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(TokenRequest codeStatusRequest )
        {
            if (codeStatusRequest == null)
            {
                return BadRequest();
            }

            AccessPermission? response;
            switch(codeStatusRequest.GrantType)
            {
                case "urn:ietf:params:oauth:grant-type:access_token":
                    response = await _deviceService.GetAccessToken(codeStatusRequest);
                    break;

                case "refresh_token":
                    var bearerExist = Request.Headers.TryGetValue(HeaderNames.Authorization, out var auth);
                    if (!bearerExist || string.IsNullOrEmpty(auth))
                    {
                        return BadRequest();
                    }

                    var refreshToken = auth.ToString().Replace("Bearer ", "");
                    response = await _deviceService.RefreshToken(codeStatusRequest, refreshToken);
                    break;

                default: return BadRequest();
            }

            if (response == null)
            {
                return NotFound();
            }

            return new OkObjectResult(response);
        }
    }
}
