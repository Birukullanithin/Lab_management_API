using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    [Table("patient", Schema = "lab")]
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("patient_id")]
        public int PatientId { get; set; }

        [JsonPropertyName("organization_id")]
        public int? OrganizationId { get; set; }

        [JsonPropertyName("legal_entity_id")]
        public int? LegalEntityId { get; set; }

        [Required]
        [MaxLength(255)]
        [JsonPropertyName("patient_name")]
        public string PatientName { get; set; } = null!;

        [JsonPropertyName("age")]
        public int? Age { get; set; }

        [MaxLength(10)]
        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [MaxLength(100)]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
