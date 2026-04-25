using System;
using System.Text.Json.Serialization;

namespace LabManagement.Models
{
    public class PatientTreeNode
    {
        [JsonPropertyName("node_id")]
        public int NodeId { get; set; }

        [JsonPropertyName("tree_id")]
        public int TreeId { get; set; }

        [JsonPropertyName("parent_node_id")]
        public int? ParentNodeId { get; set; }

        [JsonPropertyName("node_value")]
        public int NodeValue { get; set; }

        [JsonPropertyName("display_order")]
        public int DisplayOrder { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
