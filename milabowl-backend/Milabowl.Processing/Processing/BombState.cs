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

public interface IBombState
{
    ManagerBombState CalcBombStateForGw(ManagerGameWeekState managerGameWeekState);
    IList<BombGameWeekState> GetBombState();
}

public class BombState: IBombState
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

    public ManagerBombState CalcBombStateForGw(ManagerGameWeekState managerGameWeekState)
    {
        if (BombStateAlreadyCalculatedForGameWeek(managerGameWeekState.Event.GameWeek))
        {
            return _bombStateByGameWeek[managerGameWeekState.Event.GameWeek];
        }

        var roundStartBombHolder = GetRoundStartBombHolderUser(managerGameWeekState);
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
            var h2hOpponent = managerGameWeekState.User.EntryId == h2hOpponentId ? managerGameWeekState : managerGameWeekState.Opponents.First(o => o.User.EntryId == h2hOpponentId);
            bombState = new ManagerBombState(BombState: BombStateEnum.HandedOver_H2H, BombThrower: bombState.BombHolder, BombHolder: new BombHolder(
                h2hOpponent.User.EntryId,
                h2hOpponent.User.TeamName,
                h2hOpponent.User.UserName
            ));
        }
        else if (roundStartBombHolder.ActiveChip is not null && roundStartBombHolder.ActiveChip != "manager")
        {
            var playersAndScores = managerGameWeekState
                .Opponents.Select(o => new { Player = new BombHolder(o.User.EntryId, o.User.TeamName, o.User.UserName) , Score = o.TotalScore }).ToList();
            playersAndScores.Add(new { Player = new BombHolder(managerGameWeekState.User.EntryId, managerGameWeekState.User.TeamName, managerGameWeekState.User.UserName), Score = managerGameWeekState.TotalScore });
            var topScoringNonBombHolderPlayerThisRound = playersAndScores.Where(p => p.Player.FantasyManagerId != roundStartBombHolder.User.EntryId).MaxBy(o => o.Score)!.Player;
            bombState = new ManagerBombState(BombState: BombStateEnum.HandedOver_Chip, BombThrower: bombState.BombHolder, BombHolder: topScoringNonBombHolderPlayerThisRound);
        }

        if (WillBombExplode(managerGameWeekState))
        {
            bombState = bombState with { BombState = BombStateEnum.Exploded };
        }

        _bombStateByGameWeek.Add(managerGameWeekState.Event.GameWeek, bombState);
        return bombState;
    }

    private ManagerGameWeekState GetRoundStartBombHolderUser(ManagerGameWeekState managerGameWeekState)
    {
        var bombHolderId = managerGameWeekState.Event.GameWeek == 1
            ? INITIAL_BOMB_HOLDER
            : _bombStateByGameWeek[managerGameWeekState.Event.GameWeek - 1].BombHolder
                .FantasyManagerId;

        if (managerGameWeekState.User.EntryId == bombHolderId)
        {
            return managerGameWeekState;
        }

        return managerGameWeekState.Opponents.First(o => o.User.EntryId == bombHolderId);
    }

    private bool WillBombExplode(ManagerGameWeekState managerGameWeekState)
    {
        return _bombRounds.Contains(managerGameWeekState.Event.GameWeek);
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
