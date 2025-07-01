using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record LeagueDto
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("created")]
    public required DateTime Created { get; set; }

    [JsonPropertyName("closed")]
    public required bool Closed { get; set; }

    [JsonPropertyName("max_entries")]
    public required object MaxEntries { get; set; }

    [JsonPropertyName("league_type")]
    public required string LeagueType { get; set; }

    [JsonPropertyName("scoring")]
    public required string Scoring { get; set; }

    [JsonPropertyName("admin_entry")]
    public required int AdminEntry { get; set; }

    [JsonPropertyName("start_event")]
    public required int StartEvent { get; set; }

    [JsonPropertyName("code_privacy")]
    public required string CodePrivacy { get; set; }

    [JsonPropertyName("rank")]
    public required object Rank { get; set; }
}
