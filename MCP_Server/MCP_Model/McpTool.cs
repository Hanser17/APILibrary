
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCP_Server.MCP_Model
{
    public class McpTool
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
       
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("inputSchema")]
        public McpSchema InputSchema { get; set; } = new();

        [JsonPropertyName("outputSchema")]
        public McpSchema? OutputSchema { get; set; }
    }
}
