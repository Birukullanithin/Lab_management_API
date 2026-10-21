using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LabManagement.Dtos
{
    public class ResultUploadRequestDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int TestId { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
