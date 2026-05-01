using HoldemPoker.Evaluator;

public class FlopOn
{
    public static (string Action, int? Amount) DecideAction(PokerMind.Client.Model.Game game)
    {
        var (myRanking, myCategory) = rankHand(game);

        return myCategory switch
        {
            PokerHandCategory.HighCard => ("fold", null),
            PokerHandCategory.OnePair => ("call", null),
            PokerHandCategory.TwoPairs => ("call", null),
            PokerHandCategory.ThreeOfAKind => ("raise", game.RaiseAmount),
            PokerHandCategory.Straight => ("raise", game.RaiseAmount * 2),
            PokerHandCategory.Flush => ("raise", game.RaiseAmount * 3),
            PokerHandCategory.FullHouse => ("raise", game.RaiseAmount * 4),
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
}
