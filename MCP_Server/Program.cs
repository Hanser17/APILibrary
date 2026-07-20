using MCP_Server;
using MCP_Server.Authentication;
using MCP_Server.Handlers;
using MCP_Server.Internal_Models;
using MCP_Server.MCP_Model;
using MCP_Server.OpenApi;
using MCP_Server.Server;
using MCP_Server.Tools;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Controllers + JSON RPC configuration
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;

        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
    });


builder.Services
    .AddHealthChecks();
builder.Services.AddControllers();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<SwaggerLoader>();
builder.Services.AddSingleton<ToolGenerator>();
builder.Services.AddSingleton<ToolFilter>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<TokenStorage>();

builder.Services.Configure<ApiClientOptions>(
    builder.Configuration.GetSection("ApiClient"));

builder.Services.AddHttpClient<JwtTokenProvider>();

builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();

builder.Services.AddSingleton<IToolExecutor, ToolExecutor>();

builder.Services.AddSingleton<IToolRegistry, ToolRegistry>();

builder.Services.AddSingleton<ToolMapper>();

builder.Services.AddSingleton<McpServer>();

builder.Services.AddSingleton<IMcpHandler, InitializeHandler>();
builder.Services.AddSingleton<IMcpHandler, ListToolsHandler>();
builder.Services.AddSingleton<IMcpHandler, CallToolHandler>();

var app = builder.Build();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
