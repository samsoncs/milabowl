using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ElementHistoryFixtureDto
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("code")]
    public required int Code { get; init; }
    [JsonPropertyName("team_h")]
    public required int TeamH { get; init; }
    [JsonPropertyName("team_h_score")]
    public required int TeamHScore { get; init; }
    [JsonPropertyName("team_a")]
    public required int TeamA { get; init; }
    [JsonPropertyName("team_a_score")]
    public required int TeamAScore { get; init; }
    [JsonPropertyName("event")]
    public required int Event { get; init; }
    [JsonPropertyName("finished")]
    public required bool Finished { get; init; }
    [JsonPropertyName("minutes")]
    public required int Minutes { get; init; }
    [JsonPropertyName("provisional_start_time")]
    public required bool ProvisionalStartTime { get; init; }
    [JsonPropertyName("kickoff_time")]
    public required DateTime KickoffTime { get; init; }
    [JsonPropertyName("event_name")]
    public required string EventName { get; init; }
    [JsonPropertyName("is_home")]
    public required bool IsHome { get; init; }
    [JsonPropertyName("difficulty")]
    public required int Difficulty { get; init; }
}
