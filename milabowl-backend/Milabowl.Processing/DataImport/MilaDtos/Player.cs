namespace Milabowl.Processing.DataImport.MilaDtos;

public class Player
{
    public Guid PlayerId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int NowCost { get; set; }
    public int FantasyPlayerId { get; set; }
    public required Team Team { get; set; }
    public Guid FkTeamId { get; set; }
    public required IList<PlayerEvent> PlayerEvents { get; set; }
    public int Code { get; set; }
    public int ElementType { get; set; }
    public int EventPoints { get; set; }
    public required string Form { get; set; }
    public bool InDreamteam { get; set; }
    public required string News { get; set; }
    public DateTime? NewsAdded { get; set; }
    public required string Photo { get; set; }
    public required string PointsPerGame { get; set; }
    public required string SelectedByPercent { get; set; }
    public bool Special { get; set; }
    public required string Status { get; set; }
    public int TotalPoints { get; set; }
    public int TransfersIn { get; set; }
    public int TransfersInEvent { get; set; }
    public int TransfersOut { get; set; }
    public int TransfersOutEvent { get; set; }
    public required string ValueForm { get; set; }
    public required string ValueSeason { get; set; }
    public required string WebName { get; set; }
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
    public required string Influence { get; set; }
    public required string Creativity { get; set; }
    public required string Threat { get; set; }
}
