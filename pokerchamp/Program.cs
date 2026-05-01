using PokerMind.Client;
using PokerChamp;

Console.WriteLine("Starting PokerChamp...");


string API_BASE_URL = "https://pokermind.itmindsinternal.dk";
string API_KEY = "Sk1bid1 R1zz";
var client = new ApiClient(API_BASE_URL, API_KEY);

List<string> players = new List<string>
{
    "big_natty", "shrek"
};

var game = await client.StartSuite(1, players);
var suiteId = game.SuiteId;

Console.WriteLine($"Started suite with ID: {suiteId}");


var player1 = new Skibidi(client, players[0], suiteId);
var player2 = new Skibidi(client, players[1], suiteId);


Task.WaitAll(player1.Loop(), player2.Loop());
