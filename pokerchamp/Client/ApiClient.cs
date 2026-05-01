using System.Net.Http.Headers;
using System.Net.Http.Json;
using PokerMind.Client.Model;

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

    public Task<GameResponse?> NextGames(string suiteId, string playerId, CancellationToken cancellationToken = default)
    {
        var qs = $"suite_id={Uri.EscapeDataString(suiteId)}&player_id={Uri.EscapeDataString(playerId)}";
        return _httpClient.GetFromJsonAsync<GameResponse?>($"/api/next_games?{qs}", cancellationToken);
    }

    public async Task<StartSuiteResponse> StartSuite(int numGames, List<string> players, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/start_suite", new { num_games = numGames, players }, cancellationToken);
        response.EnsureSuccessStatusCode();
        var suite = await response.Content.ReadFromJsonAsync<StartSuiteResponse?>(cancellationToken);
        return suite ?? throw new InvalidOperationException("Response content was null");
    }

    public async Task PostAction(GameAction action, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/action", action, cancellationToken);

        if(!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine($"Failed to post action: {response.StatusCode}, content: {errorContent}");
        }

        response.EnsureSuccessStatusCode();
    }
}
 