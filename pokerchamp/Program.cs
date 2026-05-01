using PokerMind.Client;
using PokerChamp;

Console.WriteLine("Starting PokerChamp...");

string API_BASE_URL = "https://pokermind.itmindsinternal.dk";
string API_KEY = "Sk1bid1 R1zz";
// string API_BASE_URL = "http://localhost:4000";
// string API_KEY = "test-secret";

var client = new ApiClient(API_BASE_URL, API_KEY);

List<string> players = new List<string>
{
    "big_natty", "small_fat", "shrek", "big_boss", "simon"
};

var game = await client.StartSuite(1, players);
var suiteId = game.SuiteId;

Console.WriteLine($"Started suite with ID: {suiteId}");

var foo = players.Where(p => p != "simon").Select(playerId => new Skibidi(client, playerId, suiteId)).ToArray();


Task.WaitAll(foo.Select(s => s.Loop()).ToArray());
