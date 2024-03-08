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

public class UserGameWeek
{
    public int GameWeek => _event.GameWeek;
    public UserGameWeek? PreviousGameWeek => _previousGameWeek;
    public int UserId => _user.Id;
    public IList<PlayerEvent> Lineup { get; }
    public IList<PlayerEvent> SubsIn { get; }
    public IList<PlayerEvent> SubsOut { get; }
    public decimal TotalScore { get; }
    public decimal CumulativeTotalScore { get; }
    public decimal AvgCumulativeTotalScore { get; }
    public IList<UserGameWeek> Opponents { get; private set; } = new List<UserGameWeek>();
    public int GwPosition { get; private set; }
    public int MilaRank { get; private set; }
    public int MilaRankLastWeek { get; private set; }
    public decimal TotalCumulativeAvgMilaScore { get; private set; }

    public decimal TotalMilaScore { get; private set; }

    public decimal CumulativeTotalMilaScore { get; private set; }

    public decimal AvgCumulativeTotalMilaScore { get; private set; }

    private readonly UserGameWeek? _previousGameWeek;

    // TODO: Check - do we need separate lists for user and opponents for historic?
    private readonly IList<UserGameWeek> _historicUserGameWeeks;
    private readonly IList<UserGameWeek> _historicOpponentGameWeeks;
    private readonly Event _event;
    private readonly User _user;
    private IList<MilaRuleResult> _ruleResults = new List<MilaRuleResult>();

    public UserGameWeek(
        Event @event,
        HeadToHeadEvent headToHeadEvent,
        User user,
        IList<PlayerEvent> lineup,
        string? activeChip,
        IList<UserGameWeek> historicUserGameWeeks,
        IList<UserGameWeek> historicOpponantGameWeeks
    )
    {
        _event = @event;
        _user = user;
        Lineup = lineup;
        _historicUserGameWeeks = historicUserGameWeeks;
        _historicOpponentGameWeeks = historicOpponantGameWeeks;
        _previousGameWeek = historicUserGameWeeks.FirstOrDefault(h =>
            h.GameWeek == @event.GameWeek - 1
        );
        SubsIn = _previousGameWeek is null
            ? []
            : Lineup
                .Where(pe =>
                    _previousGameWeek.Lineup.All(ipe =>
                        ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                    )
                )
                .ToList();
        SubsOut = _previousGameWeek is null
            ? []
            : _previousGameWeek
                .Lineup.Where(pe =>
                    Lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                )
                .ToList();
        TotalScore = Lineup.Sum(l => l.TotalPoints * l.Multiplier);
        CumulativeTotalScore = historicUserGameWeeks.Sum(h => h.TotalScore) + TotalScore;
        AvgCumulativeTotalScore = Math.Round(CumulativeTotalScore / GameWeek, 2);
    }

    public void AddOpponentsForGameWeek(IList<UserGameWeek> userGameWeeks)
    {
        Opponents = userGameWeeks.Where(u => u.UserId != _user.Id).ToList();
        GwPosition = Opponents.Sum(u => u.TotalScore > TotalScore ? 1 : 0) + 1;

        AvgCumulativeTotalMilaScore = Math.Round(
            (
                _historicOpponentGameWeeks.Sum(h => h.TotalMilaScore)
                + Opponents.Sum(h => h.TotalMilaScore)
                + TotalMilaScore
            ) / (_historicOpponentGameWeeks.Count + Opponents.Count + 1),
            2
        );
    }

    public void AddRuleResult(List<MilaRuleResult> results)
    {
        _ruleResults = results;
        TotalMilaScore = results.Sum(r => r.Points);
        CumulativeTotalMilaScore =
            _historicUserGameWeeks.Sum(h => h.TotalMilaScore) + TotalMilaScore;
        TotalCumulativeAvgMilaScore = Math.Round(CumulativeTotalMilaScore / GameWeek, 2);
        MilaRank =
            Opponents.Sum(o => o.CumulativeTotalMilaScore > CumulativeTotalMilaScore ? 1 : 0) + 1;
        MilaRankLastWeek =
            _historicOpponentGameWeeks
                .Where(h => h.GameWeek == GameWeek - 1)
                .Sum(o =>
                    o.CumulativeTotalMilaScore
                    > _historicUserGameWeeks
                        .FirstOrDefault(h => h.GameWeek == GameWeek - 1)
                        ?.CumulativeTotalMilaScore
                        ? 1
                        : 0
                ) + 1;
    }

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
