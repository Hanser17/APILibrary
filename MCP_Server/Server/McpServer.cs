using MCP_Server.Handlers;
using MCP_Server.Protocol;

namespace MCP_Server.Server
{
    public class McpServer
    {
        private readonly Dictionary<string, IMcpHandler> _handlers;

        public McpServer(IEnumerable<IMcpHandler> handlers)
        {
            _handlers = handlers.ToDictionary(x => x.Method);
        }


        public async Task<JsonRpcResponse?> HandleAsync(
            JsonRpcRequest request,
            CancellationToken cancellationToken)
        {
            // MCP notifications no tienen respuesta
            if (request.Method.StartsWith("notifications/"))
            {
                return null;
            }


            if (!_handlers.TryGetValue(
                request.Method,
                out var handler))
            {
                return new JsonRpcResponse
                {
                    Id = request.Id,
                    Error = new JsonRpcError
                    {
                        Code = -32601,
                        Message = $"Method '{request.Method}' not found"
                    }
                };
            }


            return await handler.HandleAsync(
                request,
                cancellationToken);
        }
    }
}