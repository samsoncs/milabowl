using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record StatsDto
{
    [JsonPropertyName("minutes")]
    public required int Minutes { get; set; }
    [JsonPropertyName("goals_scored")]
    public required int GoalsScored { get; set; }
    [JsonPropertyName("assists")]
    public required int Assists { get; set; }
    [JsonPropertyName("clean_sheets")]
    public required int CleanSheets { get; set; }
    [JsonPropertyName("goals_conceded")]
    public required int GoalsConceded { get; set; }
    [JsonPropertyName("own_goals")]
    public required int OwnGoals { get; set; }
    [JsonPropertyName("penalties_saved")]
    public required int PenaltiesSaved { get; set; }
    [JsonPropertyName("penalties_missed")]
    public required int PenaltiesMissed { get; set; }
    [JsonPropertyName("yellow_cards")]
    public required int YellowCards { get; set; }
    [JsonPropertyName("red_cards")]
    public required int RedCards { get; set; }
    [JsonPropertyName("saves")]
    public required int Saves { get; set; }
    [JsonPropertyName("bonus")]
    public required int Bonus { get; set; }
    [JsonPropertyName("bps")]
    public required int Bps { get; set; }
    [JsonPropertyName("influence")]
    public required string Influence { get; set; }
    [JsonPropertyName("creativity")]
    public required string Creativity { get; set; }
    [JsonPropertyName("threat")]
    public required string Threat { get; set; }
    [JsonPropertyName("ict_index")]
    public required string IctIndex { get; set; }
    [JsonPropertyName("total_points")]
    public required int TotalPoints { get; set; }
    [JsonPropertyName("in_dreamteam")]
    public required bool InDreamteam { get; set; }
}
