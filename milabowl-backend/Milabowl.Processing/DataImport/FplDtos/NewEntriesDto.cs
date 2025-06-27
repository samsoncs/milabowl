using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record NewEntriesDto
{
    [JsonPropertyName("has_next")]
    public required bool HasNext { get; set; }
    [JsonPropertyName("page")]
    public required int Page { get; set; }
    [JsonPropertyName("results")]
    public required List<object> Results { get; set; }
}
