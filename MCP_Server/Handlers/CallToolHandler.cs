using MCP_Server.MCP_Model;
using MCP_Server.Protocol;
using MCP_Server.Tools;
using System.Text.Json;

namespace MCP_Server.Handlers;

public class CallToolHandler : IMcpHandler
{
    private readonly IToolRegistry _registry;
    private readonly IToolExecutor _executor;

    public string Method => JsonRpcConstants.ToolsCall;

    public CallToolHandler(
        IToolRegistry registry,
        IToolExecutor executor)
    {
        _registry = registry;
        _executor = executor;
    }

    public async Task<JsonRpcResponse> HandleAsync(
        JsonRpcRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Params is null)
            {
                return new JsonRpcResponse
                {
                    Id = request.Id,
                    Error = new JsonRpcError
                    {
                        Code = -32602,
                        Message = "Missing parameters."
                    }
                };
            }

            var toolCall = JsonSerializer.Deserialize<McpToolCall>(request.Params.ToString());


            if (toolCall is null)
            {
                return new JsonRpcResponse
                {
                    Id = request.Id,
                    Error = new JsonRpcError
                    {
                        Code = -32602,
                        Message = "Invalid parameters."
                    }
                };
            }

            if (!_registry.TryGet(toolCall.Name, out var tool))
            {
                return new JsonRpcResponse
                {
                    Id = request.Id,
                    Error = new JsonRpcError
                    {
                        Code = -32601,
                        Message = $"Tool '{toolCall.Name}' not found."
                    }
                };
            }

            var result = await _executor.ExecuteToolAsync(
                tool!,
                toolCall.Arguments,
                cancellationToken);

            return new JsonRpcResponse
            {
                Id = request.Id,
                Result = new McpToolResult
                {
                    IsError = false,
                    Text = result
                }
            };
        }
        catch (Exception ex)
        {
            return new JsonRpcResponse
            {
                Id = request.Id,
                Error = new JsonRpcError
                {
                    Code = -32603,
                    Message = ex.Message
                }
            };
        }
    }
}