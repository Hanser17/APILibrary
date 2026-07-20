using MCP_Client.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
namespace MCP_Client.OpenApi
{
    public class SwaggerLoader
    {
        private readonly HttpClient _httpClient;
        private readonly McpOptions _options;

        private OpenApiDocument? _cachedDocument;

        public SwaggerLoader(
            HttpClient httpClient,
            IOptions<McpOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<OpenApiDocument> LoadAsync(
            CancellationToken cancellationToken = default)
        {
            if (_cachedDocument is not null)
                return _cachedDocument;

            var swaggerJson = await _httpClient.GetStringAsync(
                _options.SwaggerUrl,
                cancellationToken);

            var reader = new OpenApiStringReader();

            _cachedDocument =
                reader.Read(swaggerJson, out var diagnostic);

            if (diagnostic.Errors.Any())
            {
                throw new Exception(
                    $"Error parsing swagger: {string.Join(",", diagnostic.Errors)}");
            }

            return _cachedDocument;
        }
    }
}
