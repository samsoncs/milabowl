using System.Collections.ObjectModel;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing;


// Existing bomb code Bomb
// var bombHolder = milaGameweekScores.First(b => b.UserName == _BombHolder);
// bombHolder.BombState = BombState.Holding;
//
// var bombHolderH2H = users.First(u => u.UserName == _BombHolder)
//     .HeadToHeadEvents
//     .First(l => l.Event.EventId == evt.EventId);
// if (bombHolder.ActiveChip is not null)
// {
//     var bombRecipient = milaGameweekScores.FirstOrDefault(m => m.GWPosition == bombHolder?.GWPosition - 1)
//                         ?? milaGameweekScores.OrderBy(m => m.GWPosition).Skip(1).First();
//
//     bombHolder.BombState = BombState.HandingOver_Chip;
//     bombRecipient.BombState = BombState.Receiving;
//     _BombHolder = bombRecipient.UserName;
// }
// else if (bombHolderH2H.Win == 1)
// {
//     var recipientH2H = evt.PlayerHeadToHeadEvents.FirstOrDefault(p =>
//         p.FantasyUserHeadToHeadEventID == bombHolderH2H.FantasyUserHeadToHeadEventID
//         && p.User.UserName != bombHolderH2H.User.UserName
//     );
//
//     MilaGWScore bombRecipient;
//     if(recipientH2H is not null)
//     {
//         bombRecipient =
//             milaGameweekScores.First(m => m.UserName == recipientH2H.User.UserName);
//     }
//     else
//     {
//         bombRecipient = milaGameweekScores
//             .OrderByDescending(mgw => mgw.MilaPoints)
//             .First();
//
//         if (bombRecipient.UserName == bombHolder.UserName)
//         {
//             bombRecipient = milaGameweekScores
//                 .OrderByDescending(mgw => mgw.MilaPoints)
//                 .Skip(1)
//                 .First();
//         }
//     }
//
//     bombHolder.BombState = BombState.HandingOver_H2H;
//     bombRecipient.BombState = BombState.Receiving;
//     _BombHolder = bombRecipient.UserName;
// }
//
// var roundEndBombHolder = milaGameweekScores
//     .First(b => b.BombState == BombState.Holding || b.BombState == BombState.Receiving);
// if (roundEndBombHolder.UserName == _BombHolder)
// {
//     roundEndBombHolder.BombState = BombState.Holding;
// }
//
// if (bombRounds.Contains(evt.GameWeek))
// {
//     roundEndBombHolder.BombState = BombState.Exploded;
//     roundEndBombHolder.BombPoints = -3;
//
//     var newBombHolder = milaGameweekScores.FirstOrDefault(m => m.GWPosition == bombHolder?.GWPosition - 1)
//                         ?? milaGameweekScores.OrderBy(m => m.GWPosition).Skip(1).First();
//     _BombHolder = newBombHolder.UserName;
// }

public enum BombStateEnum
{
    Receiving,
    HandingOver_Chip,
    HandingOver_H2H,
    Holding,
    Exploded
}

public record BombGameWeekState(
    BombStateEnum HolderState,
    BombStateEnum? ReceiverState
);

public record BombState(BombStateEnum);

public class BombProcessor
{
    private Dictionary<int, int> _bombHolderByGameWeek = new()
    {
        { 1, 1 },
    };
    private readonly IReadOnlyList<int> _bombRounds;

    public BombProcessor()
    {
        var random = new Random(69);
        IList<int> bombRounds = new List<int>();

        while (bombRounds.Count < 7)
        {
            var nextRandom = random.Next(2, 38);
            if (bombRounds.Contains(nextRandom))
            {
                continue;
            }

            bombRounds.Add(nextRandom);
        }

        _bombRounds = new ReadOnlyCollection<int>(bombRounds);
    }

    public bool IsBombHolder(MilaGameWeekState userGameWeek)
    {
        return userGameWeek.User.User.Id == _bombHolderByGameWeek[userGameWeek.User.Event.GameWeek];
    }

    public void CalculateBombStateForUserGameWeek(MilaGameWeekState userGameWeek)
    {
        // Outcomes
        // - Hold bomb nothing happened (no chips, or H2H wins)
        // - Recieving bomb
        // - Passing bomb after H2H win
        // - Passing bomb after chip usage
        // - Bomb exploding, passing it on


        var isBombHolder = userGameWeek.User.User.Id == _bombHolderByGameWeek[userGameWeek.User.Event.GameWeek];
        if (!isBombHolder)
        {
            return;
        }

        var bombState = BombStateEnum.Holding;

        if (userGameWeek.User.HeadToHead.CurrentUser.DidWin)
        {
            _bombHolderByGameWeek[userGameWeek.User.Event.GameWeek] =
                2; // Set bomb holder to opponent
            bombState = BombStateEnum.HandingOver_H2H;
        }

        if (_bombRounds.Contains(userGameWeek.User.Event.GameWeek))
        {
            _bombHolderByGameWeek[userGameWeek.User.Event.GameWeek] =
                2; // Set bomb holder to "next" person

        }


        // Assign current bomb holder to be the bomb holder at start of next round
        _bombHolderByGameWeek.Add(userGameWeek.User.Event.GameWeek + 1, _bombHolderByGameWeek[userGameWeek.User.Event.GameWeek]);





    }
}

public interface IRulesProcessor
{
    MilaResult CalculateForUserGameWeek(MilaGameWeekState userGameWeek);
}

public class RulesProcessor : IRulesProcessor
{
    private readonly IEnumerable<IMilaRule> _rules;

    public RulesProcessor(IEnumerable<IMilaRule> rules)
    {
        _rules = rules;
    }

    public MilaResult CalculateForUserGameWeek(MilaGameWeekState userGameWeek)
    {
        var ruleResults = _rules.Select(r => r.Calculate(userGameWeek)).ToList();
        var totalScore = ruleResults.Sum(r => r.Points);
        return new MilaResult(
            userGameWeek.User.Event.Name,
            totalScore,
            userGameWeek.User.User.TeamName,
            userGameWeek.User.User.UserName,
            userGameWeek.User.User.Id,
            userGameWeek.User.Event.GameWeek,
            ruleResults
        );
    }
}
