using System;
using System.ComponentModel.DataAnnotations;

namespace LabManagement.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public int? OrganizationId { get; set; }

        public int? LegalEntityId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PatientName { get; set; } = null!;

        public int? Age { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<string> TestNames { get; set; } = new();

        public OrderDto order { get; set; }
    }
}
