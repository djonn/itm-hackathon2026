using PokerMind.Client;
using System.Text.Json;

public class Skibidi
{

    private static readonly string API_BASE_URL = "https://pokermind.itmindsinternal.dk";
    private static readonly string API_KEY = "Sk1bid1 R1zz";

    private readonly ApiClient client;

    public Skibidi()
    {
        client = new ApiClient(API_BASE_URL, API_KEY);
    }

    public async Task Loop()
    {
        var suites = await client.NextGames("18f3da17-8e7d-4893-8f28-49e0fcde77ec", "stine");

        if(suites is null)
        {
            throw new Exception("Failed to fetch suites");
        }

        Console.WriteLine(JsonSerializer.Serialize(suites, new JsonSerializerOptions { WriteIndented = true }));

        while (true)
        {
            Console.WriteLine("Skibidi bop yes yes yes");
            Thread.Sleep(1000);
        }
    }
}