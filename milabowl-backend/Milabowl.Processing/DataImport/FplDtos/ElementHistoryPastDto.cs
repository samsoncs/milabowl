using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ElementHistoryPastDto
{
    [JsonPropertyName("season_name")]
    public required string SeasonName { get; init; }

    [JsonPropertyName("element_code")]
    public required int ElementCode { get; init; }

    [JsonPropertyName("start_cost")]
    public required int StartCost { get; init; }

    [JsonPropertyName("end_cost")]
    public required int EndCost { get; init; }

    [JsonPropertyName("total_points")]
    public required int TotalPoints { get; init; }

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
}
