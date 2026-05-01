namespace PokerMind.Client.Model;

/// <summary>
/// Suites and their associated players (<c>SuitesResponse</c> from OpenAPI: additionalProperties → string arrays).
/// </summary>
public sealed class SuitesResponse : Dictionary<string, List<string>>
{
}
