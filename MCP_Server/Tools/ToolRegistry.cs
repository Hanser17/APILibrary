using MCP_Server.Internal_Models;
using MCP_Server.OpenApi;

namespace MCP_Server.Tools
{
    public class ToolRegistry : IToolRegistry
    {
        private readonly SwaggerLoader _swaggerLoader;
        private readonly ToolGenerator _toolGenerator;
        private readonly IConfiguration _configuration;

        private readonly Dictionary<string, ToolDefinition> _tools =
            new(StringComparer.OrdinalIgnoreCase);

        public ToolRegistry(
            SwaggerLoader swaggerLoader,
            ToolGenerator toolGenerator,
            IConfiguration configuration)
        {
            _swaggerLoader = swaggerLoader;
            _toolGenerator = toolGenerator;
            _configuration = configuration;
        }
        public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var swaggerUrl =
                    _configuration["ApiClient:SwaggerUrl"]!;

                    var document =
                        await _swaggerLoader.LoadAsync(
                            swaggerUrl,
                            cancellationToken);

                    var tools =
                        _toolGenerator.Generate(document);

                    _tools.Clear();

                    foreach (var tool in tools)
                    {
                        _tools[tool.Name] = tool;
                    }

                    Console.WriteLine(
                        $"Se han registrado {_tools.Count} herramientas desde la API.");
                    return;
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine(
                        "Esperando que la API esté disponible...");

                    await Task.Delay(
                        TimeSpan.FromSeconds(2),
                        cancellationToken);
                }
            }

        }


        public ToolDefinition Get(string name)
        {
            if (!_tools.TryGetValue(name, out var tool))
            {
                throw new InvalidOperationException(
                    $"Tool '{name}' no está registrada.");
            }

            return tool;
        }

        public IReadOnlyCollection<ToolDefinition> GetAll()
        {
            return _tools.Values.ToList();
        }

        public void Register(IEnumerable<ToolDefinition> tools)
        {
            _tools.Clear();

            foreach (var tool in tools)
            {
                _tools[tool.Name] = tool;
            }
        }

        public bool TryGet(string name, out ToolDefinition? tool)
        {
            return _tools.TryGetValue(name, out tool);
        }
    }
}
