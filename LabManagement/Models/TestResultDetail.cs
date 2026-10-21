using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class TestResultDetail
    {
        [JsonPropertyName("result_detail_id")]
        public int ResultDetailId { get; set; }

        [JsonPropertyName("result_id")]
        public int ResultId { get; set; }

        [JsonPropertyName("component_name")]
        public string ComponentName { get; set; } = string.Empty;

        [JsonPropertyName("entered_value")]
        public decimal? EnteredValue { get; set; }

        [JsonPropertyName("reference_min")]
        public decimal? ReferenceMin { get; set; }

        [JsonPropertyName("reference_max")]
        public decimal? ReferenceMax { get; set; }

        [JsonPropertyName("critical_min")]
        public decimal? CriticalMin { get; set; }

        [JsonPropertyName("critical_max")]
        public decimal? CriticalMax { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "Pending";

        [JsonPropertyName("abnormal_flag")]
        public bool AbnormalFlag { get; set; }

        [JsonPropertyName("is_critical")]
        public bool IsCritical { get; set; }

        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("formula_name")]
        public string? FormulaName { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
