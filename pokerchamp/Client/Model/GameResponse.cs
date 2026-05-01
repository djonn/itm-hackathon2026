using System.Text.Json.Serialization;

namespace PokerMind.Client.Model;

/// <summary>
/// A list of upcoming games (<c>GameResponse</c> from OpenAPI).
/// </summary>
public sealed class GameResponse
{
    [JsonPropertyName("all_games_finished")]
    public bool AllGamesFinished { get; set; }

    [JsonPropertyName("games")]
    public List<Game> Games { get; set; } = [];

    [JsonPropertyName("overall_winners")]
    public List<string>? OverallWinners { get; set; }
}
