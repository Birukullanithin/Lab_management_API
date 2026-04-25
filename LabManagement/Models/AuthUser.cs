using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class AuthUser
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("organization_id")]
        public int? OrganizationId { get; set; }

        [JsonPropertyName("legal_entity_id")]
        public int? LegalEntityId { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
