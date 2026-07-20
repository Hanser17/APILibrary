using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCP_Server.MCP_Model
{
    public class McpProperty
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class McpSchema
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "object";

        [JsonPropertyName("properties")]
        public Dictionary<string, object> Properties { get; set; } = new();

        [JsonPropertyName("required")]
        public List<string> Required { get; set; }
            = new();

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; } = false;
    }
}
