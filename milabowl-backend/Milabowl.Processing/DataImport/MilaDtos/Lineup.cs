namespace Milabowl.Processing.DataImport.MilaDtos;

public class Lineup
{
    public Guid LineupId { get; set; }
    public required Event Event { get; set; }
    public Guid FkEventId { get; set; }
    public required User User { get; set; }
    public Guid FkUserId { get; set; }
    public required IList<PlayerEventLineup> PlayerEventLineups { get; set; }
    public string? ActiveChip { get; set; }
}
