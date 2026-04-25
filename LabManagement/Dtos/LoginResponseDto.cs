namespace LabManagement.Dtos
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string? Email { get; set; }

        public int? OrganizationId { get; set; }

        public int? LegalEntityId { get; set; }

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
