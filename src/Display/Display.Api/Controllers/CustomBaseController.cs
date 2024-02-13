using Display.Shared.Exceptions;
using Display.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Display.Shared.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        protected string GetRequestTenantId()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("tenantid", StringComparison.OrdinalIgnoreCase));
            return tenantClaim == null ? throw new InvalidTenantException() : tenantClaim.Value;
        }

        protected string GetRequestDeviceId()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("deviceId", StringComparison.OrdinalIgnoreCase));
            return tenantClaim == null ? throw new InvalidDeviceIdException() : tenantClaim.Value;
        }
    }
}
