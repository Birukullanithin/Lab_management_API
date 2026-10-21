using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class TestResult
    {
        [JsonPropertyName("test_result_id")]
        public int TestResultId { get; set; }

        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("patient_id")]
        public int PatientId { get; set; }

        [JsonPropertyName("test_id")]
        public int TestId { get; set; }

        [JsonPropertyName("component_name")]
        public string? ComponentName { get; set; }

        [JsonPropertyName("entered_value")]
        public decimal? EnteredValue { get; set; }

        [JsonPropertyName("calculated_value")]
        public decimal? CalculatedValue { get; set; }

        [JsonPropertyName("result_text")]
        public string? ResultText { get; set; }

        [JsonPropertyName("final_result")]
        public string? FinalResult { get; set; }

        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("reference_min")]
        public decimal? ReferenceMin { get; set; }

        [JsonPropertyName("reference_max")]
        public decimal? ReferenceMax { get; set; }

        [JsonPropertyName("abnormal_flag")]
        public bool AbnormalFlag { get; set; }

        [JsonPropertyName("critical_flag")]
        public bool CriticalFlag { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("method_used")]
        public string? MethodUsed { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("advice")]
        public string? Advice { get; set; }

        [JsonPropertyName("entry_status")]
        public string ResultStatus { get; set; } = "Pending";

        [JsonPropertyName("entered_by")]
        public int? EnteredBy { get; set; }

        [JsonPropertyName("entered_at")]
        public DateTime? EnteredAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("details")]
        public List<TestResultDetail> Details { get; set; } = new();

        [JsonPropertyName("attachments")]
        public List<TestResultAttachment> Attachments { get; set; } = new();
    }
}
