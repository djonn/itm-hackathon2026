

using PokerMind.Client.Model;

public static class Formatter
{
    public static string FormatCard(Card card)
    {
        var suit = card.Suit switch
        {
            CardSuit.Hearts => "♥",
            CardSuit.Diamonds => "♦",
            CardSuit.Clubs => "♣",
            CardSuit.Spades => "♠",
            _ => throw new ArgumentException("Unknown suit")
        };

        var rank = card.Rank switch
        {
            CardRank.Ace => "A",
            CardRank.Jack => "J",
            CardRank.Queen => "Q",
            CardRank.King => "K",
            _ => ((int)card.Rank).ToString()
        };

        return $"{suit}{rank}";
    }
}