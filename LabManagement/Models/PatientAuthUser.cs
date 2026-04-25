using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class PatientAuthUser
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        [JsonPropertyName("role_name")]
        public string? RoleName { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
