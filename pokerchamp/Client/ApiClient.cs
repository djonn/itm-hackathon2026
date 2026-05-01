using System.Net.Http.Headers;
using System.Reflection;

namespace PokerMind.Client;

public sealed class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(string baseUrl, string authorization)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(authorization);

        var normalizedBase = baseUrl.TrimEnd('/') + "/";
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(normalizedBase, UriKind.Absolute),
        };

        _httpClient.DefaultRequestHeaders.Authorization =
            AuthenticationHeaderValue.Parse(authorization);
    }

    public Task<string> NextGames(string suiteId, string playerId, CancellationToken cancellationToken = default)
    {
        var qs =
            $"suite_id={Uri.EscapeDataString(suiteId)}&player_id={Uri.EscapeDataString(playerId)}";
        return _httpClient.GetStringAsync($"/api/next_games?{qs}", cancellationToken);
    }
}
 