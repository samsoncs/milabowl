using Milabowl.Processing.DataImport.MilaDtos;
using Milabowl.Processing.Processing;

namespace Milabowl.Processing.DataImport;

public record Event(int GameWeek, string Name);

public record HeadToHeadEvent(
    int Points,
    int Win,
    int Draw,
    int Loss,
    int Total,
    bool IsKnockout,
    int LeagueId,
    bool IsBye
);

public record User(
    int Id,
    string UserName,
    string TeamName,
    int Rank,
    int LastRank,
    int EventTotal
);

public record UserGameWeek(
    Event @Event,
    HeadToHeadEvent HeadToHeadEvent,
    User User,
    IList<PlayerEvent> Lineup,
    string? ActiveChip,
    IList<UserGameWeek> HistoricUserGameWeeks,
    IList<UserGameWeek> HistoricOpponantGameWeeks
)
{
    public IList<UserGameWeek> Opponents { get; private set; } = new List<UserGameWeek>();
    private IList<MilaRuleResult> _ruleResults = new List<MilaRuleResult>();

    public void AddOpponentsForGameWeek(IList<UserGameWeek> userGameWeeks)
    {
        Opponents = userGameWeeks.Where(u => u.User.Id != User.Id).ToList();
    }

    public void AddRuleResult(List<MilaRuleResult> results)
    {
        _ruleResults = results;
    }

    public UserGameWeek? PreviousGameWeek =>
        HistoricUserGameWeeks.FirstOrDefault(h => h.Event.GameWeek == Event.GameWeek - 1);

    public List<PlayerEvent> SubsIn =>
        PreviousGameWeek is null
            ? []
            : Lineup
                .Where(pe =>
                    PreviousGameWeek.Lineup.All(ipe =>
                        ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                    )
                )
                .ToList();

    public List<PlayerEvent> SubsOut =>
        PreviousGameWeek is null
            ? []
            : PreviousGameWeek
                .Lineup.Where(pe =>
                    Lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                )
                .ToList();

    public decimal TotalScore => Lineup.Sum(l => l.TotalPoints * l.Multiplier);
    public decimal CumulativeTotalScore =>
        HistoricUserGameWeeks.Sum(h => h.TotalScore) + TotalScore;
    public decimal AvgCumulativeTotalScore => Math.Round(CumulativeTotalScore / Event.GameWeek, 2);
    public int GwPosition =>
        Opponents //.Where(u => u.User.Id != User.Id)
        .Sum(u => u.TotalScore > TotalScore ? 1 : 0) + 1;
    public decimal TotalMilaScore => _ruleResults.Sum(r => r.Points);
    public decimal CumulativeTotalMilaScore =>
        HistoricUserGameWeeks.Sum(h => h.TotalMilaScore) + TotalMilaScore;
    public decimal AvgCumulativeTotalMilaScore =>
        Math.Round(CumulativeTotalMilaScore / Event.GameWeek, 2);
    public decimal TotalCumulativeAverageMilaScore =>
        Math.Round(
            (
                HistoricOpponantGameWeeks.Sum(h => h.TotalMilaScore)
                + Opponents.Sum(h => h.TotalMilaScore)
                + TotalMilaScore
            ) / (HistoricOpponantGameWeeks.Count + Opponents.Count + 1),
            2
        );
    public int MilaRank =>
        Opponents.Sum(o => o.CumulativeTotalMilaScore > CumulativeTotalMilaScore ? 1 : 0) + 1;
    public int MilaRankLastWeek =>
        HistoricOpponantGameWeeks
            .Where(h => h.Event.GameWeek == Event.GameWeek - 1)
            .Sum(o =>
                o.CumulativeTotalMilaScore
                > HistoricUserGameWeeks
                    .FirstOrDefault(h => h.Event.GameWeek == Event.GameWeek - 1)
                    ?.CumulativeTotalMilaScore
                    ? 1
                    : 0
            ) + 1;

    public Dictionary<string, RuleResult> Rules =>
        _ruleResults.ToDictionary(k => k.RuleName, e => new RuleResult(e.Points, e.RuleShortName));
};

public record PlayerEvent(
    string FirstName,
    string Surname,
    //Guid PlayerEventId,
    int FantasyPlayerEventId,
    // Player Player,
    //Guid FkPlayerId,
    //Event Event,
    //Guid FkEventId,
    int Minutes,
    int GoalsScored,
    int Assists,
    int GoalsConceded,
    int CleanSheets,
    int PenaltiesSaved,
    int OwnGoals,
    int PenaltiesMissed,
    int YellowCards,
    int RedCards,
    int Saves,
    int Bonus,
    int Bps,
    string Influence,
    string Creativity,
    string Threat,
    string IctIndex,
    int TotalPoints,
    bool InDreamteam,
    int Multiplier,
    bool IsCaptain,
    bool IsViceCaptain,
    int PlayerPosition
);
