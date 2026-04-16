using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class Order
    {
        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("patient_id")]
        public int? PatientId { get; set; }

        [JsonPropertyName("order_number")]
        public string? OrderNumber { get; set; }

        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "Pending";

        [JsonPropertyName("total_amount")]
        public decimal? TotalAmount { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property (optional in API response)
        [JsonPropertyName("patient")]
        public Patient? Patient { get; set; }
    }
}