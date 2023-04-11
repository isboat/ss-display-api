using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Display.Api.Controllers
{
    [Route("api/display")]
    [ApiController]
    public class DisplayController : ControllerBase
    {
        [HttpGet("{tenantId}/screens/{id}")]
        public string Get(string tenantId, string id)
        {
            return "value";
        }
    }
}
