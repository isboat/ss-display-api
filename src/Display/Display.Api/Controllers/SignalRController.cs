using Display.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;

namespace Display.Api.Controllers
{
    [Route("api/signalr")]
    [ApiController]
    //[Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class SignalRController : ControllerBase
    {
        private readonly ServiceHubContext _messageHubContext;

        public SignalRController(IHubContextStore store)
        {
            _messageHubContext = store.MessageHubContext;
        }

        [HttpPost("negotiate")]
        public async Task<ActionResult> HubNegotiate([FromQuery] string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("Device ID is null or empty.");
            }

            var negotiateResponse = await _messageHubContext.NegotiateAsync(new()
            {
                UserId = SignalRExtension.ToSignalRUserId(deviceId),
                EnableDetailedErrors = true
            });

            return new JsonResult(new Dictionary<string, string>()
            {
                { "url", negotiateResponse.Url! },
                { "accessToken", negotiateResponse.AccessToken! }
            });
        }

        [HttpPost("signalr/add-to-group")]
        public async Task<IActionResult> AddToGroup([FromQuery] string deviceId, [FromQuery] string connectionId)
        {
            var tenantId = "tenantid"; // GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var connectionExist = await _messageHubContext.ClientManager.ConnectionExistsAsync(connectionId);
            if (!connectionExist)
            {
                return BadRequest("no_connection_exist");
            }

            var grp = SignalRExtension.ToGroupName(tenantId);
            var userId = SignalRExtension.ToSignalRUserId(deviceId);
            if (!await _messageHubContext.UserGroups.IsUserInGroup(userId, grp))
            {
                await _messageHubContext.UserGroups.AddToGroupAsync(userId, grp);
            }

            return NoContent();
        }
    }
}
