using MCP_Server.Internal_Models;

namespace MCP_Server.Tools
{
    public interface IToolRegistry
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);

        void Register(IEnumerable<ToolDefinition> tools);

        ToolDefinition Get(string name);

        IReadOnlyCollection<ToolDefinition> GetAll();

        bool TryGet(string name, out ToolDefinition? tool);
    }
}
