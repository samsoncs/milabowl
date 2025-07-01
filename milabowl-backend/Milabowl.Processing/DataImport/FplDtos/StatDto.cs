using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record StatDto
{
    [JsonPropertyName("identifier")]
    public required string Identifier { get; set; }

    [JsonPropertyName("points")]
    public required int Points { get; set; }

    [JsonPropertyName("value")]
    public required int Value { get; set; }
}
