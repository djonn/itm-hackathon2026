using PokerMind.Client;
using PokerMind.Client.Model;

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
        var iterationsLeft = 10;
        while (iterationsLeft-- > 0)
        {
            var game = await client.NextGames(suiteId, playerId);
            game!.Games.ForEach(PlayGame);
        }
    }

    private void PlayGame(Game game)
    {
        Console.WriteLine($"Player {playerId} is playing game {game.Id} with cards {string.Join(", ", game.Player.CurrentHand)}");
    }
}