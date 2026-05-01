using PokerMind.Client.Model;

namespace PokerBot;

public static class Bot
{
    public static (string Action, int? Amount) DecideAction(Game game)
    {
        if (game.Phase == GamePhase.PreFlop)
        {
           // return PreflopChart.GetAction(game);

            return game.Player.CurrentBet < game.BigBlindAmount ? ("call", null) : ("check", null);
        }
        
        return FlopOn.DecideAction(game);
    }
}