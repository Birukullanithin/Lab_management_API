using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    [Table("organization", Schema = "lab")]
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("organization_id")]
        public int OrganizationId { get; set; }

        [Required]
        [MaxLength(255)]
        [JsonPropertyName("organization_name")]
        public string OrganizationName { get; set; } = null!;

        [MaxLength(50)]
        [JsonPropertyName("organization_code")]
        public string? OrganizationCode { get; set; }

        [JsonPropertyName("organization_address")]
        public string? OrganizationAddress { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("organization_email")]
        public string? OrganizationEmail { get; set; }

        [MaxLength(20)]
        [JsonPropertyName("organization_phone")]
        public string? OrganizationPhone { get; set; }

        [JsonPropertyName("organization_logo")]
        public string? OrganizationLogo { get; set; }

        [JsonPropertyName("organization_description")]
        public string? OrganizationDescription { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
