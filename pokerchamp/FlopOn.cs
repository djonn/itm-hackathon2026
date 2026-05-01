using HoldemPoker.Evaluator;

public class FlopOn
{
    public static (string Action, int? Amount) DecideAction(PokerMind.Client.Model.Game game)
    {
        var playerHand = game.Player.CurrentHand.Select(c => c.ToNuget()).ToArray();
        var communityCards = game.CommunityCards.Select(c => c.ToNuget()).ToArray();

        int ranking = HoldemHandEvaluator.GetHandRanking([..playerHand, ..communityCards]);

        var log = $"""
        {string.Join("\n", playerHand.Select(c => Formatter.FormatCard(c.ToClient())))}

        {string.Join("\n", communityCards.Select(c => Formatter.FormatCard(c.ToClient())))}

        Hand ranking: {ranking}
        """;

        Console.WriteLine(log);

        return ("check", null);
    }
}