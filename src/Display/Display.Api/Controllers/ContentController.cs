using Display.Models.Exceptions;
using Display.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    [Route("api/content")]
    [ApiController]
    [Authorize] 
    public class ContentController : ControllerBase
    {
        private readonly IContentService _screenService;

        public ContentController(IContentService screenService)
        {
            _screenService = screenService;
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

        [HttpGet("screens")]
        public async Task<ActionResult> GetAll()
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var screens = await _screenService.GetScreensAsync(tenantId);
            if (screens == null || !screens.Any())
            {
                return NotFound();
            }

            return new OkObjectResult(screens);
        }

        [HttpGet("screens/{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var screen = await _screenService.GetDetailsAsync(tenantId, id);
            if (screen == null)
            {
                return NotFound();
            }

            return new OkObjectResult(screen);
        }
    }
}
