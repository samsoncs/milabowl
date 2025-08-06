using System.Collections.Concurrent;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public enum BombStateEnum
{
    Holding,
    Exploded,
    Diffused,
    HandedOver_H2H,
    HandedOver_Chip,
}

public enum BombTier
{
    Dynamite,
    Bomb,
    Nuke,
}

public record ManagerBombState(
    BombStateEnum BombState,
    BombManager BombHolder,
    BombManager? BombThrower,
    BombTier BombTier,
    int WeeksSinceLastExplosion,
    IList<BombManager> CollateralTargets,
    string? CollateralTargetPlayerName,
    IList<BombManager> BombDiffusalKits
);

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
        IList<BombManager> activeBombDiffusalKits)
    {
        if (!WillBombExplode(bombRounds, gameWeek))
        {
            return bombState;
        }

        if (activeBombDiffusalKits.Any(a => a.FantasyManagerId == bombState.BombHolder.FantasyManagerId))
        {
            return bombState with { BombState = BombStateEnum.Diffused };
        }

        return bombState with { BombState = BombStateEnum.Exploded };
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
                BombDiffusalKits = playersRewardedDiffusalKits
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
        var topScoringNonBombHolderPlayerThisRound = playersAndScores
            .Where(p => p.Player.FantasyManagerId != roundStartBombHolder.User.EntryId)
            .MaxBy(o => o.Score)!
            .Player;

        return bombState with
        {
            BombState = BombStateEnum.HandedOver_Chip,
            BombThrower = GetBombManager(roundStartBombHolder),
            BombHolder = topScoringNonBombHolderPlayerThisRound,
        };
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

public interface IBombState
{
    ManagerBombState CalcBombStateForGw(ManagerGameWeekState managerGameWeekState);
    IList<BombGameWeekState> GetBombState();
}

public class BombState : IBombState
{
    private IList<BombManager> _activeBombDiffusalKits;
    private IDictionary<int, ManagerBombState> _bombStateByGameWeek;
    private const int INITIAL_BOMB_HOLDER = 2216421;
    private IList<int> _bombRounds;

    public BombState()
    {
        _bombStateByGameWeek = new ConcurrentDictionary<int, ManagerBombState>();
        _activeBombDiffusalKits = new List<BombManager>();
        var random = new Random(69);
        _bombRounds = new List<int>();

        while (_bombRounds.Count < 7)
        {
            var nextRandom = random.Next(2, 38);
            if (_bombRounds.Contains(nextRandom))
            {
                continue;
            }

            _bombRounds.Add(nextRandom);
        }
    }

    public ManagerBombState CalcBombStateForGw(ManagerGameWeekState managerGameWeekState)
    {
        if (BombStateAlreadyCalculatedForGameWeek(managerGameWeekState.Event.GameWeek))
        {
            return _bombStateByGameWeek[managerGameWeekState.Event.GameWeek];
        }

        var roundStartBombHolder = GetRoundStartBombHolderUser(managerGameWeekState);

        var bombState = GetInitialBombState(roundStartBombHolder)
            .ProcessBombThrow(roundStartBombHolder, managerGameWeekState)
            .ProcessBombExplosion(_bombRounds, managerGameWeekState.Event.GameWeek, _activeBombDiffusalKits)
            .WithBombTier(managerGameWeekState, _bombStateByGameWeek)
            .ProcessCollateralTargets(managerGameWeekState)
            .AwardBombDiffusalKits(managerGameWeekState);

        _bombStateByGameWeek.Add(managerGameWeekState.Event.GameWeek, bombState);
        _activeBombDiffusalKits = bombState.BombDiffusalKits;
        return bombState;
    }

    private ManagerBombState GetInitialBombState(ManagerGameWeekState roundStartBombHolder)
    {
        return new ManagerBombState(
            BombStateEnum.Holding,
            GetBombHolder(roundStartBombHolder),
            null,
            BombTier.Dynamite,
            WeeksSinceLastExplosion: 0,
            CollateralTargets: [],
            CollateralTargetPlayerName: null,
            BombDiffusalKits: []
        );
    }

    private ManagerGameWeekState GetRoundStartBombHolderUser(
        ManagerGameWeekState managerGameWeekState
    )
    {
        var bombHolderId =
            managerGameWeekState.Event.GameWeek == 1
                ? INITIAL_BOMB_HOLDER
                : _bombStateByGameWeek[managerGameWeekState.Event.GameWeek - 1]
                    .BombHolder
                    .FantasyManagerId;

        if (managerGameWeekState.User.EntryId == bombHolderId)
        {
            return managerGameWeekState;
        }

        return managerGameWeekState.Opponents.First(o => o.User.EntryId == bombHolderId);
    }

    private bool BombStateAlreadyCalculatedForGameWeek(int gameWeek)
    {
        return _bombStateByGameWeek.ContainsKey(gameWeek);
    }

    public IList<BombGameWeekState> GetBombState()
    {
        return _bombStateByGameWeek
            .Select(b => new BombGameWeekState(
                b.Key,
                b.Value.BombState,
                b.Value.BombHolder,
                b.Value.BombThrower,
                b.Value.BombTier,
                b.Value.WeeksSinceLastExplosion,
                b.Value.CollateralTargets,
                b.Value.CollateralTargetPlayerName,
                b.Value.BombDiffusalKits
            ))
            .OrderBy(b => b.GameWeek)
            .ToList();
    }

    private static BombManager GetBombHolder(ManagerGameWeekState state)
    {
        return new BombManager(state.User.EntryId, state.User.TeamName, state.User.UserName);
    }
}

public record BombGameWeekState(
    int GameWeek,
    BombStateEnum BombState,
    BombManager BombHolder,
    BombManager? BombThrower,
    BombTier BombTier,
    int WeeksSinceLastExplosion,
    IList<BombManager> CollateralTargets,
    string? CollateralTargetPlayerName,
    IList<BombManager> BombDiffusalKits
);

public record BombManager(int FantasyManagerId, string ManagerName, string UserName);
