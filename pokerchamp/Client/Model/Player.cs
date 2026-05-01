using System.Text.Json.Serialization;

namespace PokerMind.Client.Model;

public class Player
{
    [JsonPropertyName("current_bet")]
    public int CurrentBet { get; set; }

    [JsonPropertyName("has_acted")]
    public bool HasActed { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("remaining_chips")]
    public int RemainingChips { get; set; }

    [JsonPropertyName("state")]
    public PlayerState State { get; set; }

    [JsonPropertyName("total_contributed")]
    public int TotalContributed { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<PlayerState>))]
public enum PlayerState
{
    [JsonStringEnumMemberName("active_in_hand")]
    ActiveInHand,

    [JsonStringEnumMemberName("inactive_in_hand")]
    InactiveInHand,

    [JsonStringEnumMemberName("all_in")]
    AllIn,

    [JsonStringEnumMemberName("out_of_chips")]
    OutOfChips,
}
