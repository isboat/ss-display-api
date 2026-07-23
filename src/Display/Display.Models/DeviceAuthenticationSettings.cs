namespace Display.Models
{
    public class DeviceAuthenticationSettings
    {
        public const string SectionName = "DeviceAuthentication";

        public string? VerificationUrl { get; set; }

        public int DeviceCodeExpiresInSeconds { get; set; } = 1800;

        public int PollingIntervalSeconds { get; set; } = 5;

        public int AccessTokenLifetimeDays { get; set; } = 3;

        public int RefreshTokenLifetimeDays { get; set; } = 365;
    }
}
