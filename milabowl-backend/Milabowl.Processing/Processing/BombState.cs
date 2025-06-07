using System.Collections.Concurrent;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;

public enum BombStateEnum
{
    Holding,
    Exploded,
    HandedOver_H2H,
    HandedOver_Chip
}

public record ManagerBombState(
    BombStateEnum BombState,
    BombHolder BombHolder,
    BombHolder? BombThrower
);

public class BombState
{
    private IDictionary<int, ManagerBombState> _bombStateByGameWeek;
    private const int INITIAL_BOMB_HOLDER = 2216421;
    private IList<int> _bombRounds;

    public BombState()
    {
        _bombStateByGameWeek = new ConcurrentDictionary<int, ManagerBombState>();
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

    public ManagerBombState CalcBombStateForGw(MilaGameWeekState milaGameWeekState)
    {
        if (BombStateAlreadyCalculatedForGameWeek(milaGameWeekState.User.Event.GameWeek))
        {
            return _bombStateByGameWeek[milaGameWeekState.User.Event.GameWeek];
        }

        var roundStartBombHolder = GetRoundStartBombHolderUser(milaGameWeekState);
        var bombState = new ManagerBombState(
            BombStateEnum.Holding,
            new BombHolder(
                roundStartBombHolder.User.EntryId,
                roundStartBombHolder.User.TeamName,
                roundStartBombHolder.User.UserName
            ),
            null
        );

        if (roundStartBombHolder.HeadToHead.CurrentUser.DidWin && roundStartBombHolder.HeadToHead.Opponent.FantasyPlayerId is not null)
        {
            var h2hOpponentId = (int)roundStartBombHolder.HeadToHead.Opponent.FantasyPlayerId!;
            var h2hOpponent = milaGameWeekState.User.User.EntryId == h2hOpponentId ? milaGameWeekState.User : milaGameWeekState.Opponents.First(o => o.User.EntryId == h2hOpponentId);
            bombState = new ManagerBombState(BombState: BombStateEnum.HandedOver_H2H, BombThrower: bombState.BombHolder, BombHolder: new BombHolder(
                h2hOpponent.User.EntryId,
                h2hOpponent.User.TeamName,
                h2hOpponent.User.UserName
            ));
        }

        if (WillBombExplode(milaGameWeekState))
        {
            bombState = bombState with { BombState = BombStateEnum.Exploded };
        }

        _bombStateByGameWeek.Add(milaGameWeekState.User.Event.GameWeek, bombState);
        return bombState;
    }

    private UserState GetRoundStartBombHolderUser(MilaGameWeekState milaGameWeekState)
    {
        var bombHolderId = milaGameWeekState.User.Event.GameWeek == 1
            ? INITIAL_BOMB_HOLDER
            : _bombStateByGameWeek[milaGameWeekState.User.Event.GameWeek - 1].BombHolder
                .FantasyManagerId;

        if (milaGameWeekState.User.User.EntryId == bombHolderId)
        {
            return milaGameWeekState.User;
        }

        return milaGameWeekState.Opponents.First(o => o.User.EntryId == bombHolderId);
    }

    private bool WillBombExplode(MilaGameWeekState milaGameWeekState)
    {
        return _bombRounds.Contains(milaGameWeekState.User.Event.GameWeek);
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
                b.Value.BombThrower
            ))
            .OrderBy(b => b.GameWeek)
            .ToList();
    }
}

public record BombGameWeekState(int GameWeek, BombStateEnum BombState, BombHolder BombHolder, BombHolder? BombThrower);
public record BombHolder(int FantasyManagerId, string ManagerName, string UserName);
