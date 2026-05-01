using PokerMind.Client.Model;

namespace PokerBot;

public static class Bot
{
    public static (string Action, int? Amount) DecideAction(Game game)
    {
        if (game.Phase == GamePhase.PreFlop)
        {
            return PreflopChart.GetAction(game);
        }
        
        if (game.Player.CurrentBet < game.BigBlindAmount)
            return ("check", null);
        else
            return ("call", null);
    }
}