using MCP_Server.Internal_Models;

namespace MCP_Server.Tools
{
    public interface IToolExecutor
    {
        Task<string> ExecuteToolAsync(ToolDefinition tool,object? arguments = null,CancellationToken cancellationToken = default);
    }
}
