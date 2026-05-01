using System.Text.Json.Serialization;

namespace PokerMind.Client.Model;

public sealed class Card
{
    [JsonPropertyName("rank")]
    public CardRank Rank { get; set; }

    [JsonPropertyName("suit")]
    public CardSuit Suit { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<CardSuit>))]
public enum CardSuit
{
    [JsonStringEnumMemberName("clubs")]
    Clubs,

    [JsonStringEnumMemberName("diamonds")]
    Diamonds,

    [JsonStringEnumMemberName("hearts")]
    Hearts,

    [JsonStringEnumMemberName("spades")]
    Spades,
}

public enum CardRank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
}
