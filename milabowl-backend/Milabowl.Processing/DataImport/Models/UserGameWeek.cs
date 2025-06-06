﻿using Milabowl.Processing.Processing;

namespace Milabowl.Processing.DataImport.Models;

public class UserGameWeek
{
    public User User { get; }
    public Event Event { get; }
    public IList<PlayerEvent> Lineup { get; }
    public IList<PlayerEvent> SubsIn { get; }
    public IList<PlayerEvent> SubsOut { get; }
    public IList<UserGameWeek> Opponents { get; private set; } = new List<UserGameWeek>();
    public HeadToHead HeadToHead { get; }
    public FplScores FplScores { get; }
    public MilaScores MilaScores { get; private set; } = null!;
    public UserGameWeek? PreviousGameWeek { get; }
    public IList<UserGameWeek> UserHistory { get; }
    public Position Position { get; private set; } = null!;
    public decimal TotalMilaScore { get; private set; }
    public decimal TotalCumulativeAvgMilaScore { get; private set; }
    public decimal CumulativeTotalMilaScore { get; private set; }
    public decimal AvgCumulativeTotalMilaScore { get; private set; }
    public Dictionary<string, RuleResult> Rules { get; private set; } = new();
    private readonly IList<UserGameWeek> _historicGameWeeks;

    public UserGameWeek(
        Event @event,
        HeadToHead headToHead,
        User user,
        IList<PlayerEvent> lineup,
        string? activeChip,
        IList<UserGameWeek> historicGameWeeks
    )
    {
        Event = @event;
        User = user;
        Lineup = lineup;
        HeadToHead = headToHead;
        _historicGameWeeks = historicGameWeeks
            .Where(e => e.Event.GameWeek < Event.GameWeek)
            .ToList();
        PreviousGameWeek = _historicGameWeeks.FirstOrDefault(h =>
            h.Event.GameWeek == @event.GameWeek - 1 && h.User.Id == User.Id
        );
        SubsIn = PreviousGameWeek is null
            ? []
            : Lineup
                .Where(pe =>
                    PreviousGameWeek.Lineup.All(ipe =>
                        ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId
                    )
                )
                .ToList();
        SubsOut = PreviousGameWeek is null
            ? []
            : PreviousGameWeek
                .Lineup.Where(pe =>
                    Lineup.All(ipe => ipe.FantasyPlayerEventId != pe.FantasyPlayerEventId)
                )
                .ToList();
        var totalScore = Lineup.Sum(l => l.TotalPoints * l.Multiplier);
        UserHistory = _historicGameWeeks.Where(h => h.User.Id == User.Id).ToList();
        var cumulativeTotalScore = UserHistory.Sum(h => h.FplScores.TotalScore) + totalScore;
        var avgCumulativeTotalScore = Math.Round(cumulativeTotalScore / Event.GameWeek, 2);
        FplScores = new FplScores(totalScore, cumulativeTotalScore, avgCumulativeTotalScore);
    }

    public void AddOpponentsForGameWeek(IList<UserGameWeek> userGameWeeks)
    {
        Opponents = userGameWeeks.Where(u => u.User.Id != User.Id).ToList();
    }

    public void AddMilaRuleResults(List<MilaRuleResult> results)
    {
        Rules = results.ToDictionary(
            k => k.RuleName,
            e => new RuleResult(e.Points, e.RuleShortName)
        );
        SetMilaScores(results);

        var gwPosition =
            Opponents.Sum(u => u.FplScores.TotalScore > FplScores.TotalScore ? 1 : 0) + 1;
        var milaRank =
            Opponents.Sum(o => o.CumulativeTotalMilaScore > CumulativeTotalMilaScore ? 1 : 0) + 1;
        var milaRankLastWeek =
            _historicGameWeeks
                .Where(h => h.User.Id != User.Id && h.Event.GameWeek == Event.GameWeek - 1)
                .Sum(o =>
                    o.CumulativeTotalMilaScore
                    > _historicGameWeeks
                        .Where(h => h.User.Id == User.Id)
                        .FirstOrDefault(h => h.Event.GameWeek == Event.GameWeek - 1)
                        ?.CumulativeTotalMilaScore
                        ? 1
                        : 0
                ) + 1;
        Position = new Position(gwPosition, milaRank, milaRankLastWeek);
    }

    private void SetMilaScores(List<MilaRuleResult> results)
    {
        var totalMilaScore = results.Sum(r => r.Points);
        var totalCumulativeAvgMilaScore = Math.Round(CumulativeTotalMilaScore / Event.GameWeek, 2);
        var cumulativeTotalMilaScore =
            _historicGameWeeks.Where(u => u.User.Id == User.Id).Sum(h => h.TotalMilaScore)
            + TotalMilaScore;
        var avgCumulativeTotalMilaScore = Math.Round(
            (
                _historicGameWeeks.Sum(h => h.TotalMilaScore)
                + Opponents.Sum(h => h.TotalMilaScore)
                + TotalMilaScore
            ) / (_historicGameWeeks.Count + Opponents.Count + 1),
            2
        );

        MilaScores = new MilaScores(
            totalMilaScore,
            totalCumulativeAvgMilaScore,
            cumulativeTotalMilaScore,
            avgCumulativeTotalMilaScore
        );
    }
};
