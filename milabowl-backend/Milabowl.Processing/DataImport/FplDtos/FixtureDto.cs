using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record FixtureDto
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }

    [JsonPropertyName("event")]
    public int? Event { get; set; }

    [JsonPropertyName("finished")]
    public required bool Finished { get; set; }

    [JsonPropertyName("finished_provisional")]
    public required bool FinishedProvisional { get; set; }

    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("kickoff_time")]
    public DateTime? KickoffTime { get; set; }

    [JsonPropertyName("minutes")]
    public required int Minutes { get; set; }

    [JsonPropertyName("provisional_start_time")]
    public required bool ProvisionalStartTime { get; set; }

    [JsonPropertyName("started")]
    public bool? Started { get; set; }

    [JsonPropertyName("team_a")]
    public required int TeamA { get; set; }

    [JsonPropertyName("team_a_score")]
    public int? TeamAScore { get; set; }

    [JsonPropertyName("team_h")]
    public required int TeamH { get; set; }

    [JsonPropertyName("team_h_score")]
    public int? TeamHScore { get; set; }

    [JsonPropertyName("team_h_difficulty")]
    public required int TeamHDifficulty { get; set; }

    [JsonPropertyName("team_a_difficulty")]
    public required int TeamADifficulty { get; set; }

    [JsonPropertyName("pulse_id")]
    public required int PulseId { get; set; }
}
