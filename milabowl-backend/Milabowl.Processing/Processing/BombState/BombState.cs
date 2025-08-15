using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState;

public interface IBombState
{
    ManagerBombState CalcBombStateForGw(ManagerGameWeekState managerGameWeekState);
    IList<BombGameWeekState> GetBombState();
}

public class BombState : IBombState
{
    private IList<BombManager> _activeBombDiffusalKits;
    private readonly IDictionary<int, ManagerBombState> _bombStateByGameWeek;
    private int InitialBombHolder => _bombSettings.InitialBombHolder;
    private readonly IList<int> _bombRounds;
    private readonly BombSettings _bombSettings;

    public BombState(IOptions<BombSettings> bombSettings)
    {
        _bombSettings = bombSettings.Value;
        _bombStateByGameWeek = new ConcurrentDictionary<int, ManagerBombState>();
        _activeBombDiffusalKits = new List<BombManager>();
        var random = new Random(_bombSettings.RandomSeed);
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
            .ProcessBombExplosion(
                _bombRounds,
                managerGameWeekState.Event.GameWeek,
                _activeBombDiffusalKits
            )
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
                ? InitialBombHolder
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
