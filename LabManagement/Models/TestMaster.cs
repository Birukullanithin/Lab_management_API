using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class TestMaster
    {
        [JsonPropertyName("test_id")]
        public int TestId { get; set; }

        [JsonPropertyName("test_name")]
        public string TestName { get; set; } = null!;

        [JsonPropertyName("test_code")]
        public string? TestCode { get; set; }

        [JsonPropertyName("normal_min")]
        public decimal? NormalMin { get; set; }

        [JsonPropertyName("normal_max")]
        public decimal? NormalMax { get; set; }

        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}