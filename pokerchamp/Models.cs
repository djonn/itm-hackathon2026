namespace PokerBot;

public record Player(string Id, int Chips);

public record StartSuiteRequest(List<Player> Players);

public record StartSuiteResponse(string SuiteId, GameState? FirstGame);

public record GameState(
    string Id,
    string Phase,
    List<string> CommunityCards,
    List<GamePlayer> Players,
    int Pot,
    int CurrentBet
);

public record GamePlayer(
    string Id,
    int Chips,
    string State,
    List<string> HoleCards,
    int Bet
);

public record ActionRequest(
    string SuiteId,
    string PlayerId,
    string Action,
    int? Amount = null
);