using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using NSubstitute;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class BombTests : MilaRuleTest<Bomb>
{
    [Fact]
    public void Should_return_minus_5_if_user_holds_exploding_bomb()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombHolder(state.User.EntryId, "Team", "User"))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(-5);
    }

    [Fact]
    public void Should_return_0_if_bomb_did_not_explode()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(
                x => x.BombState,
                f =>
                    f.PickRandom(
                        BombStateEnum.HandedOver_Chip,
                        BombStateEnum.HandedOver_H2H,
                        BombStateEnum.Holding
                    )
            )
            .RuleFor(x => x.BombHolder, new BombHolder(state.User.EntryId, "Team", "User"))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_return_0_if_bomb_exploded_on_different_person()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombHolder(state.User.EntryId + 1, "Team", "User"))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
