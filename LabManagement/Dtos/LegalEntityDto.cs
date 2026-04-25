using System;
using System.ComponentModel.DataAnnotations;

namespace LabManagement.Dtos
{
    public class LegalEntityDto
    {
        public int LegalEntityId { get; set; }

        public int? OrganizationId { get; set; }

        [MaxLength(50)]
        public string? LegalEntityCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string LegalEntityName { get; set; } = null!;

        public string? LegalEntityAddress { get; set; }

        [MaxLength(50)]
        public string? GstNumber { get; set; }

        [MaxLength(20)]
        public string? ContactNumber { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
