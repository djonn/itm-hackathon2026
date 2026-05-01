using HoldemPoker.Evaluator;
using PokerMind.Client.Model;

public class FlopOn
{
    public static (string Action, int? Amount) DecideAction(PokerMind.Client.Model.Game game)
    {

        var imLastPlayer = game.OtherPlayers.All(p => p.State == PlayerState.InactiveInHand);
        if(imLastPlayer)
        {
            return ("check", null);
        }

        var (myRanking, myCategory) = rankHand(game);

        return myCategory switch
        {
            PokerHandCategory.HighCard => ("fold", null),
            PokerHandCategory.OnePair => CallOrCheck(game),
            PokerHandCategory.TwoPairs => CallOrCheck(game),
            PokerHandCategory.ThreeOfAKind => ClampRaise(game, game.BigBlindAmount),
            PokerHandCategory.Straight => ClampRaise(game, game.BigBlindAmount * 2),
            PokerHandCategory.Flush => ClampRaise(game, game.BigBlindAmount * 3),
            PokerHandCategory.FullHouse => ClampRaise(game, game.BigBlindAmount * 4),
            PokerHandCategory.FourOfAKind => ("all_in", null),
            PokerHandCategory.StraightFlush => ("all_in", null),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static (int, PokerHandCategory) rankHand(PokerMind.Client.Model.Game game)
    {
        var playerHand = game.Player.CurrentHand.Select(c => c.ToNuget()).ToArray();
        var communityCards = game.CommunityCards.Select(c => c.ToNuget()).ToArray();

        int ranking = HoldemHandEvaluator.GetHandRanking([..playerHand, ..communityCards]);
        var category = HoldemHandEvaluator.GetHandCategory(ranking);

        var log = $"""
        {string.Join("\n", playerHand.Select(c => Formatter.FormatCard(c.ToClient())))}

        {string.Join("\n", communityCards.Select(c => Formatter.FormatCard(c.ToClient())))}

        Hand ranking: {ranking}
        Hand category: {category}
        """;

        Console.WriteLine(log);

        return (ranking, category);
    }

    private static (string, int?) ClampRaise(PokerMind.Client.Model.Game game, int raiseAmount)
    {
        var minimumRaise = game.RaiseAmount + game.HighestRaise;

        if(game.Player.RemainingChips <= minimumRaise)
        {
            return ("all_in", null);
        }

        if(raiseAmount < minimumRaise)
        {
            return CallOrCheck(game);
        }

        return ("raise", raiseAmount);
    }

    private static (string, int?) CallOrCheck(PokerMind.Client.Model.Game game)
    {
        if (game.Player.CurrentBet < game.HighestRaise)
            return ("call", null);
        else
            return ("check", null);
    }
}
