using Milabowl.Domain.Entities.Fantasy;

namespace Milabowl.Domain.Processing;

public class MilaRuleData
{
    //public Guid PlayerEventId { get; set; }
    public int FantasyPlayerEventId { get; set; }
    public Player Player { get; set; }
    //public Guid FkPlayerId { get; set; }
    //public Event Event { get; set; }
    //public Guid FkEventId { get; set; }
    public int Minutes { get; set; }
    public int GoalsScored { get; set; }
    public int Assists { get; set; }
    public int CleanSheets { get; set; }
    public int GoalsConceded { get; set; }
    public int OwnGoals { get; set; }
    public int PenaltiesSaved { get; set; }
    public int PenaltiesMissed { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int Saves { get; set; }
    public int Bonus { get; set; }
    public int Bps { get; set; }
    public string Influence { get; set; }
    public string Creativity { get; set; }
    public string Threat { get; set; }
    public string IctIndex { get; set; }
    public int TotalPoints { get; set; }
    public bool InDreamteam { get; set; }
    public int Multiplier { get; set; }
    public bool IsCaptain { get; set; }
    public int PlayerPosition { get; set; }
    public int IncreaseStreak { get; set; }
    public int EqualStreak { get; set; }
}
