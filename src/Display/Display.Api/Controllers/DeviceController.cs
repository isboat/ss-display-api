using Display.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Display.Shared.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    [Route("api/device")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)] 
    public class DeviceController : CustomBaseController
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("info")]
        public async Task<ActionResult> GetDeviceInfo()
        {
            var tenantId = GetRequestTenantId();
            var deviceid = GetRequestDeviceId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(deviceid))
            {
                return BadRequest();
            }
            
            var device = await _deviceService.GetDevice(deviceid);            
            if (device == null)
            {
                return new NotFoundObjectResult("no_such_device");
            }

            return new OkObjectResult(device);
        }
    }
}
