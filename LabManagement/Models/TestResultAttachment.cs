using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class TestResultAttachment
    {
        [JsonPropertyName("attachment_id")]
        public int AttachmentId { get; set; }

        [JsonPropertyName("result_id")]
        public int ResultId { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; } = string.Empty;

        [JsonPropertyName("content_type")]
        public string? ContentType { get; set; }

        [JsonPropertyName("file_size")]
        public long FileSize { get; set; }

        [JsonPropertyName("file_content")]
        public byte[]? FileContent { get; set; }

        [JsonPropertyName("uploaded_at")]
        public DateTime UploadedAt { get; set; }
    }
}
