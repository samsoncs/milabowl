using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;
using Milabowl.Processing.Processing.BombState;
using Milabowl.Processing.Processing.BombState.Models;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using NSubstitute;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class BombTests : MilaRuleTest<Bomb>
{
    [Fact]
    public void Should_return_minus_2_if_user_holds_exploding_dynamite()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Dynamite)
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(-2);
    }

    [Fact]
    public void Should_return_minus_4_if_user_holds_exploding_bomb()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Bomb)
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(-4);
    }

    [Fact]
    public void Should_not_be_hit_by_collateral_if_bomb_tier_is_dynamite()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId + 1, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Dynamite)
            .RuleFor(x => x.CollateralTargets, [new BombManager(state.User.EntryId, "", "", 1)])
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_return_minus_half_if_user_is_collateral_of_exploding_bomb()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId + 1, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Bomb)
            .RuleFor(x => x.CollateralTargets, [new BombManager(state.User.EntryId, "", "", 1)])
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(-2);
    }

    [Fact]
    public void Should_return_minus_half_if_user_is_collateral_of_exploding_nuke()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId + 1, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Nuke)
            .RuleFor(x => x.CollateralTargets, [new BombManager(state.User.EntryId, "", "", 1)])
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(-3);
    }

    [Fact]
    public void Should_return_minus_6_if_user_holds_exploding_nuke()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Nuke)
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(-6);
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
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId, "Team", "User", 1))
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
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId + 1, "Team", "User", 1))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_get_6point9_points_for_diffusing_a_bomb()
    {
        var state = new ManagerGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory
            .GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Diffused)
            .RuleFor(x => x.BombHolder, new BombManager(state.User.EntryId, "Team", "User", 1))
            .RuleFor(x => x.BombTier, BombTier.Dynamite)
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<ManagerGameWeekState>()).Returns(managerBombState);
        var rule = new Bomb(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.ShouldBe(6.9m);
    }
}
