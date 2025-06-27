using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record PickDto
{
    [JsonPropertyName("element")]
    public required int Element { get; set; }
    [JsonPropertyName("position")]
    public required int Position { get; set; }
    [JsonPropertyName("multiplier")]
    public required int Multiplier { get; set; }
    [JsonPropertyName("is_captain")]
    public required bool IsCaptain { get; set; }
    [JsonPropertyName("is_vice_captain")]
    public required bool IsViceCaptain { get; set; }
}
