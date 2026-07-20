using MCP_Server.Protocol;
using MCP_Server.Server;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MCP_Server.Transport.Http;

[ApiController]
[Route("api/mcp")]
public class McpController : ControllerBase
{
    private readonly McpServer _server;

    public McpController(McpServer server)
    {
        _server = server;
    }



    [HttpPost]
    public async Task<IActionResult> Handle(
        [FromBody] JsonRpcRequest request,
        CancellationToken cancellationToken)
    {

        var response = await _server.HandleAsync(
            request,
            cancellationToken);


        if (response == null)
        {
           
            return Ok();
        }

        Console.WriteLine(
            System.Text.Json.JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));

        return Ok(response);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "MCP Server running",
            protocol = "JSON-RPC"
        });
    }
}