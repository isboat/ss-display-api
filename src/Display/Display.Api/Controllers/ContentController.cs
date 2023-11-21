using Display.Shared.Exceptions;
using Display.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Display.Shared.Constants;
using Display.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    [Route("api/content")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)] 
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService screenService)
        {
            _contentService = screenService;
        }

        [HttpGet("data")]
        public async Task<ActionResult> GetData()
        {
            var tenantId = GetRequestTenantId();
            var deviceid = GetRequestDeviceId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(deviceid))
            {
                return BadRequest();
            }
            var device = await _contentService.GetDeviceAsync(deviceid);
            if (device == null)
            {
                return new NotFoundObjectResult("no such device");
            }
            if (string.IsNullOrEmpty(device.ScreenId))
            {
                return new NotFoundObjectResult("no screen id is null");
            }

            var screen = await _contentService.GetDetailsAsync(tenantId, device.ScreenId);
            if (screen == null)
            {
                return NotFound();
            }

            return new OkObjectResult(screen);
        }

        private string GetRequestTenantId()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("tenantid", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                throw new InvalidTenantException();
            }

            return tenantClaim.Value;
        }

        private string GetRequestDeviceId()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("deviceId", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                throw new InvalidDeviceIdException();
            }

            return tenantClaim.Value;
        }
    }
}
