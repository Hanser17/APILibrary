

namespace MCP_Server.MCP_Model
{
    public class McpToolCall
    {
        public string Name { get; set; } = string.Empty;

        public Dictionary<string, object?> Arguments { get; set; }
            = new();
    }
}
