using System.ComponentModel.DataAnnotations;

namespace LabManagement.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;
    }
}
