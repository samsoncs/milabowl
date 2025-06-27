using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record EventRootDto
{
    [JsonPropertyName("elements")]
    public required List<ElementDto> Elements { get; set; }
}
