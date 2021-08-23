using System;
using System.Text.Json.Serialization;

namespace Milabowl.Business.DTOs.Import
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("deadline_time")]
        public DateTime DeadlineTime { get; set; }
        public bool Finished { get; set; }
        [JsonPropertyName("data_checked")]
        public bool DataChecked { get; set; }
        [JsonPropertyName("is_previous")]
        public bool IsPrevious { get; set; }
        [JsonPropertyName("is_current")]
        public bool IsCurrent { get; set; }
        [JsonPropertyName("is_next")]
        public bool IsNext { get; set; }
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
}
