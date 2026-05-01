using System.Text.Json;
using System.Text.Json.Serialization;
using PokerMind.Client.Model;

namespace PokerBot;

public static class PreflopChart
{
    private static readonly PreflopChartDefinition ChartDefinition = LoadChartDefinition();
    private static readonly ActionSettingsDefinition ActionSettings = LoadActionSettings();

    public static (string Action, int? Amount) GetAction(Game game)
    {
        if (game.Phase != GamePhase.PreFlop || game.Player.CurrentHand.Count != 2)
            return Fallback(game);

        var position = ResolvePosition(game);
        var hand = HandNotation.FromCards(game.Player.CurrentHand);
        var chartValue = LookupValue(position, hand);

        return DetermineAction(game, chartValue);
    }

    private static (string Action, int? Amount) DetermineAction(Game game, int value)
    {
        var (action, _) = ActionSettings.GetAction(value);

        return action switch
        {
            // Only fold if not matching the current bet
            "fold" => game.HighestRaise == game.Player.CurrentBet ? ("check", null) : ("fold", null),
            // Only check if not facing a raise, else fold
            "check" => game.Player.CurrentBet < game.HighestRaise ? ("fold", null) : ("check", null),
            // Only call if not matching the current bet, else check
            "call" => game.HighestRaise == game.Player.CurrentBet ? ("check", null) : ("call", null),
            "raise" => CalculateRaise(game, value),
            _ => Fallback(game),
        };
    }

    private static (string Action, int? Amount) CalculateRaise(Game game, int value)
    {
        var raiseMultiplier = ActionSettings.RaiseConfig.BaseMultiplier;

        if (ActionSettings.RaiseConfig.ScaleWithValue)
        {
            raiseMultiplier = 1.5 + ((value - ActionSettings.RaiseConfig.RaiseThreshold) / 100.0) * 1.5;
        }

        var initialRaise = game.BigBlindAmount * raiseMultiplier;
        bool shouldCall = initialRaise < game.HighestRaise;
        if (shouldCall)
            return ("call", null);

        bool canRaiseWithInitial = initialRaise > game.HighestRaise + game.RaiseAmount;
        if (!canRaiseWithInitial)
            return ("call", null);
        
        var raiseAmount = (int)Math.Max(initialRaise, game.RaiseAmount);
        if (raiseAmount >= game.Player.RemainingChips)
            return ("all_in", game.Player.RemainingChips);

        return ("raise", raiseAmount);
    }

    private static int LookupValue(string position, string hand)
    {
        if (ChartDefinition.Positions.TryGetValue(position, out var chart)
            && chart.TryGetValue(hand, out var value))
        {
            return value;
        }

        if (ChartDefinition.Positions.TryGetValue("Any", out var anyChart)
            && anyChart.TryGetValue(hand, out var anyValue))
        {
            return anyValue;
        }

        return ChartDefinition.DefaultValue;
    }

    private static string ResolvePosition(Game game)
    {
        var ids = new List<string> { game.Player.Id };
        ids.AddRange(game.OtherPlayers.Select(p => p.Id));

        if (ids.Count == 2 && !string.IsNullOrEmpty(game.SmallBlindId))
        {
            return game.Player.Id == game.SmallBlindId ? "SB" : "BB";
        }

        var rotated = RotateToSmallBlind(ids, game.SmallBlindId);
        if (rotated.Count == 0)
            return "Any";

        var index = rotated.IndexOf(game.Player.Id);
        if (index < 0)
            return "Any";

        return index switch
        {
            0 => "SB",
            1 => "BB",
            2 => "Button",
            3 => "Cutoff",
            4 => "Middle",
            5 => "Hijack",
            6 => "UTG",
            _ => "Early",
        };
    }

    private static List<string> RotateToSmallBlind(List<string> ids, string smallBlindId)
    {
        if (string.IsNullOrEmpty(smallBlindId) || !ids.Contains(smallBlindId))
            return ids;

        var index = ids.IndexOf(smallBlindId);
        return ids.Skip(index).Concat(ids.Take(index)).ToList();
    }

    private static (string Action, int? Amount) Fallback(Game game)
    {
        if (game.Player.CurrentBet == 0)
            return ("check", null);

        return ("call", null);
    }

    private static PreflopChartDefinition LoadChartDefinition()
    {
        var fileName = "preflop-charts.json";
        var searchPaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, fileName),
            Path.Combine(Environment.CurrentDirectory, fileName),
            Path.Combine(AppContext.BaseDirectory, "..", fileName),
        };

        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<PreflopChartDefinition>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    }) ?? new PreflopChartDefinition();
                }
                catch
                {
                    break;
                }
            }
        }

        return new PreflopChartDefinition();
    }

    private static ActionSettingsDefinition LoadActionSettings()
    {
        var fileName = "action-settings.json";
        var searchPaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, fileName),
            Path.Combine(Environment.CurrentDirectory, fileName),
            Path.Combine(AppContext.BaseDirectory, "..", fileName),
        };

        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<ActionSettingsDefinition>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    }) ?? new ActionSettingsDefinition();
                }
                catch
                {
                    break;
                }
            }
        }

        return new ActionSettingsDefinition();
    }
}

public sealed class PreflopChartDefinition
{
    [JsonPropertyName("default_value")]
    public int DefaultValue { get; set; } = 50;

    [JsonPropertyName("positions")]
    public Dictionary<string, Dictionary<string, int>> Positions { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class ActionSettingsDefinition
{
    [JsonPropertyName("action_thresholds")]
    public Dictionary<string, ActionThreshold> ActionThresholds { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        ["fold"] = new ActionThreshold { Min = 0, Max = 19 },
        ["check"] = new ActionThreshold { Min = 20, Max = 49 },
        ["call"] = new ActionThreshold { Min = 50, Max = 79 },
        ["raise"] = new ActionThreshold { Min = 80, Max = 100 }
    };

    [JsonPropertyName("raise_config")]
    public RaiseConfig RaiseConfig { get; set; } = new();

    public (string Action, ActionThreshold Threshold) GetAction(int value)
    {
        foreach (var (action, threshold) in ActionThresholds)
        {
            if (value >= threshold.Min && value <= threshold.Max)
                return (action, threshold);
        }

        return ("call", ActionThresholds["call"]);
    }
}

public sealed class ActionThreshold
{
    [JsonPropertyName("min")]
    public int Min { get; set; }

    [JsonPropertyName("max")]
    public int Max { get; set; }
}

public sealed class RaiseConfig
{
    [JsonPropertyName("base_multiplier")]
    public double BaseMultiplier { get; set; } = 2.0;

    [JsonPropertyName("scale_with_value")]
    public bool ScaleWithValue { get; set; } = true;

    [JsonIgnore]
    public int RaiseThreshold => 80;
}

public static class HandNotation
{
    private static readonly string[] RankStrings =
    {
        "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2"
    };

    public static string FromCards(List<Card> cards)
    {
        if (cards.Count != 2)
            return string.Empty;

        var first = cards[0];
        var second = cards[1];

        var rank1 = RankString(first.Rank);
        var rank2 = RankString(second.Rank);

        var suited = first.Suit == second.Suit ? "s" : "o";

        if (first.Rank == second.Rank)
            return rank1 + rank2;

        return string.CompareOrdinal(rank1, rank2) > 0
            ? rank1 + rank2 + suited
            : rank2 + rank1 + suited;
    }

    private static string RankString(CardRank rank) => rank switch
    {
        CardRank.Ace => "A",
        CardRank.King => "K",
        CardRank.Queen => "Q",
        CardRank.Jack => "J",
        CardRank.Ten => "T",
        CardRank.Nine => "9",
        CardRank.Eight => "8",
        CardRank.Seven => "7",
        CardRank.Six => "6",
        CardRank.Five => "5",
        CardRank.Four => "4",
        CardRank.Three => "3",
        CardRank.Two => "2",
        _ => string.Empty,
    };
}
