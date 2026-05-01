using PokerBot;

namespace tests;

public class UnitTest1
{
    [Fact]
    public void DecideAction_Check_WhenNoBet()
    {
        var game = new GameState("id", "phase", new List<string>(), new List<GamePlayer>(), 0, 0);
        var action = Bot.DecideAction(game);
        Assert.Equal("check", action.Action);
        Assert.Null(action.Amount);
    }

    [Fact]
    public void DecideAction_Call_WhenBetPending()
    {
        var game = new GameState("id", "phase", new List<string>(), new List<GamePlayer>(), 0, 100);
        var action = Bot.DecideAction(game);
        Assert.Equal("call", action.Action);
        Assert.Null(action.Amount);
    }
}
