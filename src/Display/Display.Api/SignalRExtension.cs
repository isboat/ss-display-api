namespace Display.Api
{
    public class SignalRConstants
    {
        public const string ClientSideTargetEvent = "ReceiveChangeMessage";

        public const string AzureSignalRConnectionStringName = "AzureSignalRConnectionString";

        public const string HubName = "changelistenerhub";
    }

    public class SignalRExtension
    {
        public static string ToSignalRUserId(string deviceId)
        {
            return $"device_id_{deviceId}";
        }

        public static string ToGroupName(string tenantId)
        {
            return $"grp-{tenantId}";
        }
    }
}
