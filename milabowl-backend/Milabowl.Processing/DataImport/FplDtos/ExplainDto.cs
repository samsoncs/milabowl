using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ExplainDto
{
    [JsonPropertyName("fixture")]
    public required int Fixture { get; set; }
    [JsonPropertyName("stats")]
    public required List<StatDto> Stats { get; set; }
}
