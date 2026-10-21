using System;

namespace LabManagement.Dtos
{
    public class ResultAttachmentDto
    {
        public int AttachmentId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string? ContentType { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
