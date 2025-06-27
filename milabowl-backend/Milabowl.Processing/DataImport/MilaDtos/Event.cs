using System.ComponentModel.DataAnnotations;

namespace Milabowl.Processing.DataImport.MilaDtos;

public class Event
{
    [Key]
    public Guid EventId { get; set; }
    public int FantasyEventId { get; set; }
    public DateTime Deadline { get; set; }
    public bool Finished { get; set; }
    public bool DataChecked { get; set; }
    public required string Name { get; set; }
    public int GameWeek { get; set; }
    public int? MostSelectedPlayerId { get; set; }
    public int? MostTransferredInPlayerId { get; set; }
    public int? MostCaptainedPlayerId { get; set; }
    public int? MostViceCaptainedPlayerId { get; set; }

    public required IList<PlayerEvent> PlayerEvents { get; set; }
    public required IList<UserHeadToHeadEvent> PlayerHeadToHeadEvents { get; set; }
    public required IList<Lineup> Lineups { get; set; }
    public required IList<Fixture> Fixtures { get; set; }
}
