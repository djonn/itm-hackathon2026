using HoldemPoker.Evaluator;
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
        
        return FlopOn.DecideAction(game);
    }
}