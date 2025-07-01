using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ElementHistoryDto
{
    [JsonPropertyName("element")]
    public required int Element { get; init; }

    [JsonPropertyName("fixture")]
    public required int Fixture { get; init; }

    [JsonPropertyName("opponent_team")]
    public required int OpponentTeam { get; init; }

    [JsonPropertyName("total_points")]
    public required int TotalPoints { get; init; }

    [JsonPropertyName("was_home")]
    public required bool WasHome { get; init; }

    [JsonPropertyName("kickoff_time")]
    public required DateTime KickoffTime { get; init; }

    [JsonPropertyName("team_h_score")]
    public required int? TeamHScore { get; init; }

    [JsonPropertyName("team_a_score")]
    public required int? TeamAScore { get; init; }

    [JsonPropertyName("round")]
    public required int Round { get; init; }

    [JsonPropertyName("minutes")]
    public required int Minutes { get; init; }

    [JsonPropertyName("goals_scored")]
    public required int GoalsScored { get; init; }

    [JsonPropertyName("assists")]
    public required int Assists { get; init; }

    [JsonPropertyName("clean_sheets")]
    public required int CleanSheets { get; init; }

    [JsonPropertyName("goals_conceded")]
    public required int GoalsConceded { get; init; }

    [JsonPropertyName("own_goals")]
    public required int OwnGoals { get; init; }

    [JsonPropertyName("penalties_saved")]
    public required int PenaltiesSaved { get; init; }

    [JsonPropertyName("penalties_missed")]
    public required int PenaltiesMissed { get; init; }

    [JsonPropertyName("yellow_cards")]
    public required int YellowCards { get; init; }

    [JsonPropertyName("red_cards")]
    public required int RedCards { get; init; }

    [JsonPropertyName("saves")]
    public required int Saves { get; init; }

    [JsonPropertyName("bonus")]
    public required int Bonus { get; init; }

    [JsonPropertyName("bps")]
    public required int Bps { get; init; }

    [JsonPropertyName("influence")]
    public required string Influence { get; init; }

    [JsonPropertyName("creativity")]
    public required string Creativity { get; init; }

    [JsonPropertyName("threat")]
    public required string Threat { get; init; }

    [JsonPropertyName("ict_index")]
    public required string IctIndex { get; init; }

    [JsonPropertyName("value")]
    public required int Value { get; init; }

    [JsonPropertyName("transfers_balance")]
    public required int TransfersBalance { get; init; }

    [JsonPropertyName("selected")]
    public required int Selected { get; init; }

    [JsonPropertyName("transfers_in")]
    public required int TransfersIn { get; init; }

    [JsonPropertyName("transfers_out")]
    public required int TransfersOut { get; init; }
}
