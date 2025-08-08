using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState;

public static class ManagerBombStateExtensions
{
    public static ManagerBombState ProcessBombThrow(
        this ManagerBombState bombState,
        ManagerGameWeekState roundStartBombHolder,
        ManagerGameWeekState managerGameWeekState
    )
    {
        return bombState
            .AttemptHeadToHeadBombThrow(roundStartBombHolder, managerGameWeekState)
            .AttemptChipBombThrow(roundStartBombHolder, managerGameWeekState);
    }

    public static ManagerBombState ProcessBombExplosion(
        this ManagerBombState bombState,
        IList<int> bombRounds,
        int gameWeek,
        IList<BombManager> activeBombDiffusalKits
    )
    {
        if (!WillBombExplode(bombRounds, gameWeek))
        {
            return bombState;
        }

        if (
            activeBombDiffusalKits.Any(a =>
                a.FantasyManagerId == bombState.BombHolder.FantasyManagerId
            )
        )
        {
            return bombState with { BombState = BombStateEnum.Diffused };
        }

        return bombState with
        {
            BombState = BombStateEnum.Exploded,
        };
    }

    public static ManagerBombState WithBombTier(
        this ManagerBombState bombState,
        ManagerGameWeekState managerGameWeekState,
        IDictionary<int, ManagerBombState> bombStateByGameWeek
    )
    {
        var weeksSinceLastExplosion =
            managerGameWeekState.Event.GameWeek
            - bombStateByGameWeek
                .Where(b => b.Value.BombState == BombStateEnum.Exploded)
                .Select(b => b.Key)
                .DefaultIfEmpty(0)
                .Max();
        var bombTier = weeksSinceLastExplosion switch
        {
            < 4 => BombTier.Dynamite,
            < 7 => BombTier.Bomb,
            _ => BombTier.Nuke,
        };

        return bombState with
        {
            BombTier = bombTier,
            WeeksSinceLastExplosion =
            bombState.BombState == BombStateEnum.Exploded ? 0 : weeksSinceLastExplosion,
        };
    }

    public static ManagerBombState ProcessCollateralTargets(
        this ManagerBombState bombState,
        ManagerGameWeekState managerGameWeekState
    )
    {
        if (bombState.BombState != BombStateEnum.Exploded)
        {
            return bombState;
        }

        var allPlayers = managerGameWeekState
            .Opponents.Select(o => o)
            .Append(managerGameWeekState)
            .ToList();
        var bombHolderManager = allPlayers.First(ap =>
            ap.User.EntryId == bombState.BombHolder.FantasyManagerId
        );
        var viceCaptain = bombHolderManager.Lineup.First(l => l.IsViceCaptain);
        var collateralTargets = allPlayers
            .Where(o =>
                o.User.EntryId != bombHolderManager.User.EntryId
                && o.Lineup.First(l => l.IsCaptain).FantasyPlayerEventId
                == viceCaptain.FantasyPlayerEventId
            )
            .Select(GetBombManager)
            .ToList();

        return bombState with
        {
            CollateralTargets = collateralTargets,
            CollateralTargetPlayerName = viceCaptain.WebName,
        };
    }

    public static ManagerBombState AwardBombDiffusalKits(
        this ManagerBombState bombState,
        ManagerGameWeekState managerGameWeekState
    )
    {
        var playersRewardedDiffusalKits = managerGameWeekState
            .Opponents.Select(o => o)
            .Append(managerGameWeekState)
            .Where(m => m.TotalScore > 99)
            .Select(GetBombManager)
            .ToList();

        return bombState with
        {
            BombDiffusalKits = playersRewardedDiffusalKits,
        };
    }

    private static bool WillBombExplode(IList<int> bombRounds, int gameWeek)
    {
        return bombRounds.Contains(gameWeek);
    }

    private static ManagerBombState AttemptHeadToHeadBombThrow(
        this ManagerBombState bombState,
        ManagerGameWeekState roundStartBombHolder,
        ManagerGameWeekState managerGameWeekState
    )
    {
        if (!DidWinH2H(roundStartBombHolder))
        {
            return bombState;
        }

        var h2hOpponentId = (int)roundStartBombHolder.HeadToHead.Opponent.FantasyPlayerId!;
        var h2hOpponent =
            managerGameWeekState.User.EntryId == h2hOpponentId
                ? managerGameWeekState
                : managerGameWeekState.Opponents.First(o => o.User.EntryId == h2hOpponentId);

        return bombState with
        {
            BombState = BombStateEnum.HandedOver_H2H,
            BombThrower = GetBombManager(roundStartBombHolder),
            BombHolder = GetBombManager(h2hOpponent),
        };
    }

    private static bool DidWinH2H(ManagerGameWeekState roundStartBombHolder)
    {
        return roundStartBombHolder.HeadToHead.CurrentUser.DidWin
               && roundStartBombHolder.HeadToHead.Opponent.FantasyPlayerId is not null;
    }

    private static ManagerBombState AttemptChipBombThrow(
        this ManagerBombState bombState,
        ManagerGameWeekState roundStartBombHolder,
        ManagerGameWeekState managerGameWeekState
    )
    {
        if (!DidUseChip(roundStartBombHolder))
        {
            return bombState;
        }

        var playersAndScores = managerGameWeekState
            .Opponents.Select(o => new { Player = GetBombManager(o), Score = o.TotalScore })
            .ToList();
        
        playersAndScores.Add(
            new
            {
                Player = GetBombManager(managerGameWeekState),
                Score = managerGameWeekState.TotalScore,
            }
        );
        var topScoringNonBombHolderPlayersThisRound = playersAndScores
            .Where(p => p.Player.FantasyManagerId != roundStartBombHolder.User.EntryId)
            .GroupBy(p => p.Score)
            .OrderByDescending(p => p.Key)
            .First()
            .Select(p => p.Player)
            .ToList();

        if (IsTiedTopScorer(topScoringNonBombHolderPlayersThisRound))
        {
            return bombState;
        }

        return bombState with
        {
            BombState = BombStateEnum.HandedOver_Chip,
            BombThrower = GetBombManager(roundStartBombHolder),
            BombHolder = topScoringNonBombHolderPlayersThisRound.First(),
        };
    }

    private static bool IsTiedTopScorer(
        IList<BombManager> topScoringNonBombHolderPlayersThisRound
    )
    {
        return topScoringNonBombHolderPlayersThisRound.Count != 1;
    }

    private static bool DidUseChip(ManagerGameWeekState roundStartBombHolder)
    {
        return roundStartBombHolder.ActiveChip is not null
               && roundStartBombHolder.ActiveChip != "manager";
    }

    private static BombManager GetBombManager(ManagerGameWeekState state)
    {
        return new BombManager(state.User.EntryId, state.User.TeamName, state.User.UserName);
    }
}
