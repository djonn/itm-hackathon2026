using PokerMind.Client;
using System.Text.Json;

public class Skibidi
{

    private static readonly string API_KEY = "Sk1bid1 R1zz";

    private readonly ApiClient client;

    public Skibidi()
    {
    }

    public async Task Loop()
    {

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