using System.Text.Json.Serialization;

namespace PokerMind.Client.Model;

public sealed class Game
{
    [JsonPropertyName("big_blind_amount")]
    public int BigBlindAmount { get; set; }

    [JsonPropertyName("community_cards")]
    public List<Card> CommunityCards { get; set; } = [];

    [JsonPropertyName("current_player_id")]
    public string CurrentPlayerId { get; set; } = "";

    [JsonPropertyName("hands_played")]
    public int HandsPlayed { get; set; }

    [JsonPropertyName("highest_raise")]
    public int HighestRaise { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("other_players")]
    public List<Player> OtherPlayers { get; set; } = [];

    [JsonPropertyName("phase")]
    public GamePhase Phase { get; set; }

    [JsonPropertyName("player")]
    public GamePerspectivePlayer Player { get; set; } = null!;

    [JsonPropertyName("pot")]
    public int Pot { get; set; }

    [JsonPropertyName("raise_amount")]
    public int RaiseAmount { get; set; }

    [JsonPropertyName("small_blind_id")]
    public string SmallBlindId { get; set; } = "";

    [JsonPropertyName("winner")]
    public string? Winner { get; set; }
}

public sealed class GamePerspectivePlayer : Player
{
    [JsonPropertyName("current_hand")]
    public List<Card> CurrentHand { get; set; } = [];
}

[JsonConverter(typeof(JsonStringEnumConverter<GamePhase>))]
public enum GamePhase
{
    [JsonStringEnumMemberName("pre_flop")]
    PreFlop,

    [JsonStringEnumMemberName("flop")]
    Flop,

    [JsonStringEnumMemberName("turn")]
    Turn,

    [JsonStringEnumMemberName("river")]
    River,

    [JsonStringEnumMemberName("game_finished")]
    GameFinished,
}
