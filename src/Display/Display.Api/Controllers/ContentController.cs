using Display.Shared.Exceptions;
using Display.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Display.Shared.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    [Route("api/content")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)] 
    public class ContentController : CustomBaseController
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
                return new NotFoundObjectResult("no_such_device");
            }

            if (string.IsNullOrEmpty(device.ScreenId))
            {
                return new NotFoundObjectResult("no_screen_id");
            }

            var screen = await _contentService.GetDetailsAsync(tenantId, device.ScreenId);
            if (screen == null)
            {
                return new NotFoundObjectResult("no_screen_data_found");
            }

            return new OkObjectResult(screen);
        }
    }
}
