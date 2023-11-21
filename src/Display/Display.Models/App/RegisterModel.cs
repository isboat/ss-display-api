using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Models.App
{
    [BsonIgnoreExtraElements]
    public class DeviceCodeRegistrationModel
    {
        public DateTime? RegisteredDatetime { get; set; }

        public DateTime? ApprovedDatetime { get; set; }

        public string? TenantId { get; set; }
        public string? Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? UserCode { get; set; }
        public int? ExpiresIn { get; set; }
        public int? Interval { get; set; }
        public string? DeviceName { get; set; }
        public string? ScreenId { get; set; }
    }

    public class DeviceCodeModel
    {
        [JsonProperty("device_code")]
        public string? DeviceCode { get; set; }

        [JsonProperty("user_code")]
        public string? UserCode { get; set; }

        [JsonProperty("verification_url")]
        public string? VerificationUrl { get; set; }

        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonProperty("interval")]
        public int? Interval { get; set; }

        [JsonProperty("device_name")]
        public string? DeviceName { get; set; }
        public string? ClientId { get; set; }
    }

    public class CodeRequest
    {
        [JsonProperty("client_id")]
        public string? ClientId { get; set; }


        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }
    }

    public class TokenRequest
    {
        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }

        [JsonProperty("device_code")]
        public string? DeviceCode { get; set; }


        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }
    }

    public class AccessPermission
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public double? ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }

        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
