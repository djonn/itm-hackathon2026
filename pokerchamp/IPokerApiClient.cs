namespace PokerBot;

public interface IPokerApiClient
{
    Task<StartSuiteResponse> StartSuiteAsync(StartSuiteRequest request);
    Task<List<GameState>> GetNextGamesAsync(string suiteId, string playerId);
    Task<GameState> SubmitActionAsync(ActionRequest request);
    Task CloseSuiteAsync(string suiteId);
}