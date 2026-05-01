using System.Text.Json.Serialization;

namespace PokerMind.Client.Model;

public sealed class GameAction
{
    [JsonPropertyName("action")]
    public required ActionType TheTypeThatWeWantToDoOfAction { get; set; }

    [JsonPropertyName("game_id")]
    public required string GameId { get; set; }

    [JsonPropertyName("player_id")]
    public required string PlayerId { get; set; }

    [JsonPropertyName("amount")]
    public int? Amount { get; set; }

    public static GameAction New(string actionType, int? amount, string gameId, string playerId)
    {
        return new GameAction
        {
            TheTypeThatWeWantToDoOfAction = actionType switch
            {
                "fold" => ActionType.Fold,
                "check" => ActionType.Check,
                "call" => ActionType.Call,
                "all_in" => ActionType.AllIn,
                "raise" => ActionType.Raise,
                _ => throw new ArgumentException($"Invalid action type: {actionType}")
            },
            Amount = amount,
            GameId = gameId,
            PlayerId = playerId
        };
    }
}

[JsonConverter(typeof(JsonStringEnumConverter<ActionType>))]
public enum ActionType
{
    [JsonStringEnumMemberName("fold")]
    Fold,

    [JsonStringEnumMemberName("check")]
    Check,

    [JsonStringEnumMemberName("call")]
    Call,

    [JsonStringEnumMemberName("all_in")]
    AllIn,

    [JsonStringEnumMemberName("raise")]
    Raise,
}
