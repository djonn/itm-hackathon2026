using System.Text.Json.Serialization;

namespace PokerMind.Client.Model;

public sealed class StartSuiteResponse
{
    [JsonPropertyName("suite_id")]
    public string SuiteId { get; set; } = string.Empty;
}
