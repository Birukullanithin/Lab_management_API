using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class PatientTree
    {
        [JsonPropertyName("tree_id")]
        public int TreeId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
