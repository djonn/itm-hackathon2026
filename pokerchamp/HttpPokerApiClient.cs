using System.Net.Http.Json;

namespace PokerBot;

public class HttpPokerApiClient : IPokerApiClient
{
    private readonly HttpClient _httpClient;

    public HttpPokerApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:4000")
        };
        _httpClient.DefaultRequestHeaders.Add("Authorization", "test-secret");
    }

    public async Task<StartSuiteResponse> StartSuiteAsync(StartSuiteRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/start_suite", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StartSuiteResponse>() ?? throw new InvalidOperationException("Invalid response");
    }

    public async Task<List<GameState>> GetNextGamesAsync(string suiteId, string playerId)
    {
        var response = await _httpClient.GetAsync($"api/next_games?suite_id={suiteId}&player_id={playerId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<GameState>>() ?? new List<GameState>();
    }

    public async Task<GameState> SubmitActionAsync(ActionRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/action", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GameState>() ?? throw new InvalidOperationException("Invalid response");
    }

    public async Task CloseSuiteAsync(string suiteId)
    {
        var response = await _httpClient.DeleteAsync($"api/close_suite?suite_id={suiteId}");
        response.EnsureSuccessStatusCode();
    }
}