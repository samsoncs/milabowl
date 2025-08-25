using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public class BombDiffusalEarnedEventGenerator : IBombEventGenerator
{
    public bool IsApplicable(ManagerBombState bombState)
    {
        return bombState.BombState != BombStateEnum.Diffused
               && bombState.BombState != BombStateEnum.Exploded;
    }

    public BombHistoryRow GetRow(ManagerBombState bombState)
    {
        return new BombHistoryRow(
            $"**{string.Join(", ", bombState.BombDiffusalKits.Select(c => c.ManagerName))}** eaned a diffusal kit!",
            $"{BombEmoji.DiffusalKit}",
            BombEventRowSeverity.Info
        );
    }
}
