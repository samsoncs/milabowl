namespace Milabowl.Domain.Entities.Fantasy;

public class PlayerEventLineup
{
    public Guid PlayerEventLineupId { get; set; }
    public PlayerEvent PlayerEvent { get; set; }
    public Guid FkPlayerEventId { get; set; }
    public Lineup Lineup { get; set; }
    public Guid FkLineupId { get; set; }
    public int Multiplier { get; set; }
    public bool IsCaptain { get; set; }
    public bool IsViceCaptain { get; set; }

}