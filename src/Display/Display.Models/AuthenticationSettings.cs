namespace Display.Models
{
    public class AuthenticationSettings
    {
        public const string SectionName = "Authentication";

        public bool Disabled { get; set; }

        public string DisabledTenantId { get; set; } = "development-tenant";

        public string DisabledDeviceId { get; set; } = "development-device";
    }
}
