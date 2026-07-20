

namespace MCP_Client.Abstractions
{
    public interface IToolProvider
    {
        Task<IReadOnlyCollection<McpToolDefinition>> GetToolsAsync(CancellationToken cancellationToken = default);

        Task<McpToolDefinition?> GetToolAsync(string toolName,CancellationToken cancellationToken = default);
    }
}
