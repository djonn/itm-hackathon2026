using HoldemPoker.Cards;
using PokerMind.Client.Model;

public static class CardExtensions
{
    public static HoldemPoker.Cards.Card ToNuget(this PokerMind.Client.Model.Card from)
    {
        var type = from.Rank switch
        {
            CardRank.Two => CardType.Deuce,
            CardRank.Three => CardType.Three,
            CardRank.Four => CardType.Four,
            CardRank.Five => CardType.Five,
            CardRank.Six => CardType.Six,
            CardRank.Seven => CardType.Seven,
            CardRank.Eight => CardType.Eight,
            CardRank.Nine => CardType.Nine,
            CardRank.Ten => CardType.Ten,
            CardRank.Jack => CardType.Jack,
            CardRank.Queen => CardType.Queen,
            CardRank.King => CardType.King,
            CardRank.Ace => CardType.Ace,
            _ => throw new ArgumentOutOfRangeException(nameof(from.Rank), $"Unexpected card type: {from.Rank}")
        };

        var color = from.Suit switch
        {
            CardSuit.Clubs => CardColor.Club,
            CardSuit.Diamonds => CardColor.Diamond,
            CardSuit.Hearts => CardColor.Heart,
            CardSuit.Spades => CardColor.Spade,
            _ => throw new ArgumentOutOfRangeException(nameof(from.Suit), $"Unexpected card color: {from.Suit}")
        };

        return new HoldemPoker.Cards.Card(type, color);
    }

    public static PokerMind.Client.Model.Card ToClient(this HoldemPoker.Cards.Card from)
    {
        var rank = from.Type switch
        {
            CardType.Deuce => CardRank.Two,
            CardType.Three => CardRank.Three,
            CardType.Four => CardRank.Four,
            CardType.Five => CardRank.Five,
            CardType.Six => CardRank.Six,
            CardType.Seven => CardRank.Seven,
            CardType.Eight => CardRank.Eight,
            CardType.Nine => CardRank.Nine,
            CardType.Ten => CardRank.Ten,
            CardType.Jack => CardRank.Jack,
            CardType.Queen => CardRank.Queen,
            CardType.King => CardRank.King,
            CardType.Ace => CardRank.Ace,
            _ => throw new ArgumentOutOfRangeException(nameof(from.Type), $"Unexpected card type: {from.Type}")
        };

        var suit = from.Color switch
        {
            CardColor.Club => CardSuit.Clubs,
            CardColor.Diamond => CardSuit.Diamonds,
            CardColor.Heart => CardSuit.Hearts,
            CardColor.Spade => CardSuit.Spades,
            _ => throw new ArgumentOutOfRangeException(nameof(from.Color), $"Unexpected card color: {from.Color}")
        };

        return new PokerMind.Client.Model.Card(){Rank = rank, Suit = suit};
    }

}