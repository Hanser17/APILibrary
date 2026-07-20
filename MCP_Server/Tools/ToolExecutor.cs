

using MCP_Server.Authentication;
using MCP_Server.Internal_Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MCP_Server.Tools
{
    public class ToolExecutor : IToolExecutor
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenProvider _tokenProvider;
        private readonly ApiClientOptions _options;

        public ToolExecutor(
            HttpClient httpClient,
            ITokenProvider tokenProvider,
            IOptions<ApiClientOptions> options)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _options = options.Value;
            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        }

        public async Task<string> ExecuteToolAsync(
            ToolDefinition tool,
            object? body = null,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(
                new HttpMethod(tool.HttpMethod),
                tool.Route);

            
            if (tool.RequiresAuthentication)
            {
                var token = await _tokenProvider.GetTokenAsync(cancellationToken);

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

          
            if (body is not null)
            {
                request.Content = JsonContent.Create(body);
            }

            var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
