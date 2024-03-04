using Display.Models.App;
using Display.Services;
using Display.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;

namespace Display.Api.Controllers
{
    [Route("api/signalr")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class SignalRController : CustomBaseController
    {
        private readonly ServiceHubContext _messageHubContext;
        private readonly ISignalrConnectionService _signalrConnectionService;

        public SignalRController(IHubContextStore store, ISignalrConnectionService signalrConnectionService)
        {
            _messageHubContext = store.MessageHubContext;
            _signalrConnectionService = signalrConnectionService;
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

        [HttpPost("add-connection")]
        public async Task<IActionResult> AddConnection([FromQuery] string deviceId, [FromQuery] string deviceName, [FromQuery] string connectionId)
        {
            var tenantId = GetRequestTenantId();

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

            await _signalrConnectionService.AddAsync(tenantId, new SignalrConnectionModel 
            { 
                Id = connectionId,
                DeviceId = deviceId,
                TenantId = tenantId,
                DeviceName = deviceName,
                ConnectionDateTime = DateTime.UtcNow
            });

            return NoContent();
        }

        [HttpPost("remove-connection")]
        public async Task<IActionResult> RemoveConnection([FromQuery] string deviceId, [FromQuery] string connectionId)
        {
            var tenantId = GetRequestTenantId();

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
            if (await _messageHubContext.UserGroups.IsUserInGroup(userId, grp))
            {
                await _messageHubContext.UserGroups.RemoveFromGroupAsync(userId, grp);
                await _signalrConnectionService.RemoveAsync(tenantId, new SignalrConnectionModel
                {
                    Id = connectionId,
                    DeviceId = deviceId,
                    TenantId = tenantId
                });
            }

            return NoContent();
        }
    }
}
