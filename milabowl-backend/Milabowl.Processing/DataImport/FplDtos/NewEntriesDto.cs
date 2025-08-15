using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record NewEntriesResultDto
{
    [JsonPropertyName("entry")]
    public required int Entry { get; set; }
    [JsonPropertyName("entry_name")]
    public required string EntryName { get; set; }
    [JsonPropertyName("joined_time")]
    public required DateTime JoinedTime { get; set; }
    [JsonPropertyName("player_first_name")]
    public required string PlayerFirstName { get; set; }
    [JsonPropertyName("player_last_name")]
    public required string PlayerLastName { get; set; }
}

public record NewEntriesDto
{
    [JsonPropertyName("has_next")]
    public required bool HasNext { get; set; }

    [JsonPropertyName("page")]
    public required int Page { get; set; }

    [JsonPropertyName("results")]
    public required List<NewEntriesResultDto> Results { get; set; }
}
