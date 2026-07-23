namespace Display.Models
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public string? SigningKey { get; set; }
    }
}
