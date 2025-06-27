using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record EventDto
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("deadline_time")]
    public required DateTime DeadlineTime { get; set; }

    [JsonPropertyName("finished")]
    public required bool Finished { get; set; }

    [JsonPropertyName("data_checked")]
    public required bool DataChecked { get; set; }

    [JsonPropertyName("is_previous")]
    public required bool IsPrevious { get; set; }

    [JsonPropertyName("is_current")]
    public required bool IsCurrent { get; set; }

    [JsonPropertyName("is_next")]
    public required bool IsNext { get; set; }

    [JsonPropertyName("most_selected")]
    public int? MostSelected { get; set; }

    [JsonPropertyName("most_transferred_in")]
    public int? MostTransferredIn { get; set; }

    [JsonPropertyName("top_element")]
    public int? TopElement { get; set; }

    [JsonPropertyName("transfers_made")]
    public int TransfersMade { get; set; }

    [JsonPropertyName("most_captained")]
    public int? MostCaptained { get; set; }

    [JsonPropertyName("most_vice_captained")]
    public int? MostViceCaptained { get; set; }
}
