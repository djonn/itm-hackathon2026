using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using PokerMind.Client;
using System.Text.Json;

public class Skibidi
{

    private static readonly string API_KEY = "Sk1bid1 R1zz";

    private readonly ApiClient client;

    public Skibidi()
    {
        var authProvider = new ApiKeyAuthenticationProvider(API_KEY, "authorization", ApiKeyAuthenticationProvider.KeyLocation.Header);
        var adapter = new HttpClientRequestAdapter(authProvider);
        client = new ApiClient(adapter);
    }

    public async Task Loop()
    {
        var suites = await client.Api.Suites.GetAsync();

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