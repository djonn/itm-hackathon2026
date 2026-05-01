using PokerBot;

Console.WriteLine("Starting Poker Bot...");

var client = new HttpPokerApiClient();

try
{
    var request = new StartSuiteRequest([new("player1", 10000), new("player2", 10000)]);
    var response = await client.StartSuiteAsync(request);
    Console.WriteLine($"Started suite {response.SuiteId}");

    string suiteId = response.SuiteId;
    string playerId = "player1";

    while (true)
    {
        var games = await client.GetNextGamesAsync(suiteId, playerId);
        if (games.Count == 0)
        {
            Console.WriteLine("No more games, closing suite");
            await client.CloseSuiteAsync(suiteId);
            break;
        }

        foreach (var game in games)
        {
            var action = Bot.DecideAction(game);
            var actionRequest = new ActionRequest(suiteId, playerId, action.Action, action.Amount);
            try
            {
                var newState = await client.SubmitActionAsync(actionRequest);
                Console.WriteLine($"Submitted {action.Action} for game {game.Id}, new phase {newState.Phase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting action: {ex.Message}");
            }
        }

        await Task.Delay(1000); // poll every 1s
    }

    Console.WriteLine("Bot finished.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
