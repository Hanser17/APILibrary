using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
namespace MCP_Server.OpenApi
{
    public class SwaggerLoader
    {
        private readonly HttpClient _httpClient;

        public SwaggerLoader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenApiDocument> LoadAsync(
            string swaggerUrl,
            CancellationToken cancellationToken = default)
        {
            var swaggerJson = await _httpClient.GetStringAsync(
                swaggerUrl,
                cancellationToken);

            var reader = new OpenApiStringReader();

            var document = reader.Read(
                swaggerJson,
                out var diagnostic);

            if (diagnostic.Errors != null && diagnostic.Errors.Any()) // Added null check for Errors
            {
                throw new Exception(
                    $"Error parsing OpenAPI document: {string.Join(",", diagnostic.Errors)}");
            }

            return document;
        }
    }
}
