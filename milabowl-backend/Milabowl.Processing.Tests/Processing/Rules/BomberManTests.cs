using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;
using NSubstitute;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class BomberManTests : MilaRuleTest<BomberMan>
{
    [Fact]
    public void Should_return_5_if_user_is_bomb_thrower_and_bomb_explodes()
    {
        var state = new MilaGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory.GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombThrower, new BombHolder(state.User.User.EntryId, "Team", "User"))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<MilaGameWeekState>()).Returns(managerBombState);
        var rule = new BomberMan(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.Should().Be(5);
    }

    [Fact]
    public void Should_return_0_if_bomb_did_not_explode()
    {
        var state = new MilaGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory.GetManagerBombState()
            .RuleFor(x => x.BombState, f => f.PickRandom(BombStateEnum.Holding, BombStateEnum.HandedOver_Chip, BombStateEnum.HandedOver_H2H))
            .RuleFor(x => x.BombThrower, new BombHolder(state.User.User.EntryId, "Team", "User"))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<MilaGameWeekState>()).Returns(managerBombState);
        var rule = new BomberMan(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_return_0_if_bomb_exploded_but_different_thrower()
    {
        var state = new MilaGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory.GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombThrower, new BombHolder(state.User.User.EntryId+1, "Team", "User"))
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<MilaGameWeekState>()).Returns(managerBombState);
        var rule = new BomberMan(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_return_0_if_bomb_exploded_but_no_thrower()
    {
        var state = new MilaGameWeekStateBuilder().Build();
        var bombStateMock = Substitute.For<IBombState>();
        var managerBombState = TestStateFactory.GetManagerBombState()
            .RuleFor(x => x.BombState, BombStateEnum.Exploded)
            .RuleFor(x => x.BombThrower, (BombHolder?)null)
            .Generate();
        bombStateMock.CalcBombStateForGw(Arg.Any<MilaGameWeekState>()).Returns(managerBombState);
        var rule = new BomberMan(bombStateMock);

        var result = rule.Calculate(state);

        result.Points.Should().Be(0);
    }
}

