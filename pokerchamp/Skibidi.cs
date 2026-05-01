using PokerBot;
using PokerMind.Client;
using PokerMind.Client.Model;

namespace PokerChamp;

public class Skibidi
{
    private readonly ApiClient client;
    private readonly string playerId;
    private readonly string suiteId;

    public Skibidi(ApiClient client, string playerId, string suiteId)
    {
        this.client = client;
        this.playerId = playerId;
        this.suiteId = suiteId;
    }

    public async Task Loop()
    {
        GameResponse? nextGamesResponse = await client.NextGames(suiteId, playerId) ?? throw new Exception("Received null response from NextGames");
        do
        {
            nextGamesResponse = await client.NextGames(suiteId, playerId) ?? throw new Exception("Received null response from NextGames");

            var playGameTasks = nextGamesResponse!.Games.Select(PlayGame);
            await Task.WhenAll(playGameTasks);
        } while (!nextGamesResponse.AllGamesFinished);

        Console.WriteLine($"{playerId} sees that all games are finished.");
        Console.WriteLine($"Winner: {string.Join(", ", nextGamesResponse.OverallWinners ?? [])}");
    }

    private Task PlayGame(Game game)
    {
        Console.WriteLine($"[{game.Player.Id}] Deciding action...");

        var (action, amount) = Bot.DecideAction(game);
        Console.WriteLine($"[{game.Player.Id}] Decided {action} {(amount.HasValue ? amount.Value.ToString() : "")}");

        var gameAction = GameAction.New(action, amount, game.Id, playerId);
        return client.PostAction(gameAction);
    }
}