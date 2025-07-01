using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ElementDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("stats")]
    public required StatsDto Stats { get; init; }

    [JsonPropertyName("explain")]
    public required List<ExplainDto> Explain { get; init; }
}
