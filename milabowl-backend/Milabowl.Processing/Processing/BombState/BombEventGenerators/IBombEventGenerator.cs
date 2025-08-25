using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public interface IBombEventGenerator
{
    bool IsApplicable(ManagerBombState bombState);
    BombHistoryRow GetRow(ManagerBombState bombState);
}