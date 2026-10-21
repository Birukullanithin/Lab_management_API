using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabManagement.Dtos
{
    public class ResultSaveRequestDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int TestId { get; set; }

        public List<ResultComponentDto> Components { get; set; } = new();

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [MaxLength(1000)]
        public string? Advice { get; set; }

        [MaxLength(50)]
        public string ResultStatus { get; set; } = "Pending";
    }
}
