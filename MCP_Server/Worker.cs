using MCP_Server.Authentication;
using MCP_Server.MCP_Model;
using MCP_Server.OpenApi;
using MCP_Server.Protocol;
using MCP_Server.Server;
using MCP_Server.Tools;
using System.Text.Json;

namespace MCP_Server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IToolRegistry _toolRegistry;
    private readonly McpServer _mcpServer;
    private readonly IToolExecutor _toolExecutor;

    public Worker(
        ILogger<Worker> logger,
        IToolRegistry toolRegistry,
        McpServer mcpServer,
        IToolExecutor toolExecutor)
    {
        _logger = logger;
        _toolRegistry = toolRegistry;
        _toolExecutor = toolExecutor;
        _mcpServer = mcpServer;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            await _toolRegistry.InitializeAsync(stoppingToken);


            var initializeRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = JsonRpcConstants.Initialize
            };

            var initializeResponse = await _mcpServer.HandleAsync(
                initializeRequest,
                stoppingToken);

            _logger.LogInformation(
                JsonSerializer.Serialize(
                    initializeResponse,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));

            var request = new JsonRpcRequest
            {
                Id = 1,
                Method = JsonRpcConstants.ToolsList
            };

            var response = await _mcpServer.HandleAsync(
                request,
                stoppingToken);

            _logger.LogInformation(
                JsonSerializer.Serialize(
                    response,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));


            //////////////////////

            var requestcall = new JsonRpcRequest
            {
                Id = 2,
                Method = JsonRpcConstants.ToolsCall,
                Params = JsonSerializer.SerializeToElement(
                    new McpToolCall
                    {
                        Name = "GetLibros"
                    })
            };

            var responsecall = await _mcpServer.HandleAsync(
                  requestcall,
                  stoppingToken);

            _logger.LogInformation(JsonSerializer.Serialize(responsecall,
              new JsonSerializerOptions
              {
                 WriteIndented = true
              }));

            //_logger.LogInformation(
            //    "Se registraron {Count} tools.",
            //    _toolRegistry.GetAll().Count);

            //var tool = _toolRegistry.Get("GetLibros");

            //var result = await _toolExecutor.ExecuteToolAsync(
            //    tool,
            //    cancellationToken: stoppingToken);

            //_logger.LogInformation(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error ejecutando el MCP.");
        }
    }
}