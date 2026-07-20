

using Microsoft.OpenApi.Models;

namespace MCP_Client.Abstractions
{
    public interface ISwaggerLoader
    {
        
        Task<OpenApiDocument> LoadAsync(
        CancellationToken cancellationToken = default);
    }
}
