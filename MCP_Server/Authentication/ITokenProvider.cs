

namespace MCP_Server.Authentication
{
    public interface ITokenProvider
    {
        Task<string> GetTokenAsync(
    CancellationToken cancellationToken = default);


        Task<string> RefreshTokenAsync(
            CancellationToken cancellationToken = default);

    }
}
