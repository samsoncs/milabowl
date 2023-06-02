namespace Milabowl.Domain.Entities.Fantasy;

public class Lineup
{
    public Guid LineupId { get; set; }
    public Event Event { get; set; }
    public Guid FkEventId { get; set; }
    public User User { get; set; }
    public Guid FkUserId { get; set; }
    public IList<PlayerEventLineup> PlayerEventLineups { get; set; }
    public string? ActiveChip { get; set; }
}