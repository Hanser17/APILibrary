using MCP_Server.Internal_Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace MCP_Server.Authentication;

public class JwtTokenProvider : ITokenProvider
{
    private readonly HttpClient _httpClient;
    private readonly ApiClientOptions _options;
    private readonly TokenStorage _storage;


    public JwtTokenProvider(
        HttpClient httpClient,
        IOptions<ApiClientOptions> options,
        TokenStorage storage)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _storage = storage;

        _httpClient.BaseAddress =
            new Uri(_options.BaseUrl);
    }


    public async Task<string> GetTokenAsync(
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(_storage.JWToken))
        {
            return _storage.JWToken;
        }


        return await LoginAsync(cancellationToken);
    }

    public async Task<string> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
     
        if (string.IsNullOrEmpty(_storage.RefreshToken))
        {
            return await LoginAsync(cancellationToken);
        }


        var response =
            await _httpClient.PostAsJsonAsync(
                "/api/Login/refresh-token",
                new
                {
                    refreshToken = _storage.RefreshToken
                },
                cancellationToken);


        if (!response.IsSuccessStatusCode)
        {
            
            return await LoginAsync(cancellationToken);
        }


        var result =
            await response.Content
            .ReadFromJsonAsync<RefreshTokeResponse>(
                cancellationToken);


        _storage.JWToken = result!.Token;
        _storage.RefreshToken = result.RefreshTaken;


        return _storage.JWToken;
    }
    

    private async Task<string> LoginAsync(
        CancellationToken cancellationToken)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "/api/Login/Login-user",
                new
                {
                    username = _options.Username,
                    password = _options.Password
                },
                cancellationToken);


        response.EnsureSuccessStatusCode();


        var loginResult =
            await response.Content
            .ReadFromJsonAsync<LoginResponse>(
                cancellationToken);


        _storage.JWToken = loginResult!.JWToken;
        _storage.RefreshToken = loginResult.RefreshToken;


        return _storage.JWToken;
    }
}