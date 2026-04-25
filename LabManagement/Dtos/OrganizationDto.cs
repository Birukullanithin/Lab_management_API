using System;
using System.ComponentModel.DataAnnotations;

namespace LabManagement.Dtos
{
    public class OrganizationDto
    {
        public int OrganizationId { get; set; }

        [Required]
        [MaxLength(255)]
        public string OrganizationName { get; set; } = null!;

        [MaxLength(50)]
        public string? OrganizationCode { get; set; }

        public string? OrganizationAddress { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? OrganizationEmail { get; set; }

        [MaxLength(20)]
        public string? OrganizationPhone { get; set; }

        public string? OrganizationLogo { get; set; }

        public string? OrganizationDescription { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
