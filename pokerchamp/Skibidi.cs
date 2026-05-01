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
        while (true)
        {
            var nextGamesResponse = await client.NextGames(suiteId, playerId) ?? throw new Exception("Received null response from NextGames");

            if (nextGamesResponse.AllGamesFinished)
            {
                Console.WriteLine($"{playerId} sees that all games are finished.");
                break;
            }

            var playGameTasks = nextGamesResponse!.Games.Select(PlayGame);
            await Task.WhenAll(playGameTasks);
        }
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