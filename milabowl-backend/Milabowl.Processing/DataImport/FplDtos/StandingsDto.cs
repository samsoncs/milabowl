using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record StandingsDto
{
    [JsonPropertyName("has_next")]
    public required bool HasNext { get; set; }
    [JsonPropertyName("page")]
    public required int Page { get; set; }
    [JsonPropertyName("results")]
    public required List<ResultDto> Results { get; set; }
}
