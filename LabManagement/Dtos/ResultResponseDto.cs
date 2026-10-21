using System;
using System.Collections.Generic;

namespace LabManagement.Dtos
{
    public class ResultResponseDto
    {
        public int ResultId { get; set; }

        public int OrderId { get; set; }

        public int PatientId { get; set; }

        public int TestId { get; set; }

        public string? OrderNumber { get; set; }

        public string? PatientName { get; set; }

        public string TestName { get; set; } = string.Empty;

        public string ResultStatus { get; set; } = "Pending";

        public bool HasCriticalAlert { get; set; }

        public string? Remarks { get; set; }

        public string? Advice { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<ResultComponentDto> Components { get; set; } = new();

        public List<ResultAttachmentDto> Attachments { get; set; } = new();
    }
}
