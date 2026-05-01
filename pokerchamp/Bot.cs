using HoldemPoker.Evaluator;
using PokerMind.Client.Model;

namespace PokerBot;

public static class Bot
{
    public static (string Action, int? Amount) DecideAction(Game game)
    {
        var playerHand = game.Player.CurrentHand.Select(c => c.ToNuget()).ToArray();
        var communityCards = game.CommunityCards.Select(c => c.ToNuget()).ToArray();

        var log = $"""
        {string.Join("\n", playerHand.Select(c => Formatter.FormatCard(c.ToClient())))}

        {string.Join("\n", communityCards.Select(c => Formatter.FormatCard(c.ToClient())))}
        
        Current bet: {game.Player.CurrentBet}
        Highest raise: {game.HighestRaise}
        """;

        Console.WriteLine(log);
        
        if (game.Phase == GamePhase.PreFlop)
        {
           // return PreflopChart.GetAction(game);

            return game.Player.CurrentBet < game.BigBlindAmount ? ("call", null) : ("check", null);
        }
        
        // Simple strategy: if no bet pending (current bet == 0), check; else call
        if (game.Player.CurrentBet < game.HighestRaise)
            if (game.HighestRaise > game.Player.RemainingChips)
                return ("all_in", game.Player.RemainingChips);

        return ("check", null);
        // return FlopOn.DecideAction(game);
    }
}