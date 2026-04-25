using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    [Table("legal_entity", Schema = "lab")]
    public class LegalEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("legal_entity_id")]
        public int LegalEntityId { get; set; }

        [JsonPropertyName("organization_id")]
        public int? OrganizationId { get; set; }

        [MaxLength(50)]
        [JsonPropertyName("legal_entity_code")]
        public string? LegalEntityCode { get; set; }

        [Required]
        [MaxLength(255)]
        [JsonPropertyName("legal_entity_name")]
        public string LegalEntityName { get; set; } = null!;

        [JsonPropertyName("legal_entity_address")]
        public string? LegalEntityAddress { get; set; }

        [MaxLength(50)]
        [JsonPropertyName("gst_number")]
        public string? GstNumber { get; set; }

        [MaxLength(20)]
        [JsonPropertyName("contact_number")]
        public string? ContactNumber { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
