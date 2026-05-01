using PokerMind.Client.Model;

namespace PokerBot;

public static class Bot
{
    public static (string Action, int? Amount) DecideAction(Game game)
    {
        // Simple strategy: if no bet pending (current bet == 0), check; else call
        if (game.Player.CurrentBet == 0)
            return ("check", null);
        else
            return ("call", null);
    }
}