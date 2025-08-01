using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class UniqueCapTests : MilaRuleTest<UniqueCap>
{
    [Fact]
    public void Should_award_2_points_if_user_has_unique_captain()
    {
        var opponent = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetCaptain().RuleFor(x => x.FantasyPlayerEventId, 1))
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetCaptain()
                    .RuleFor(x => x.FantasyPlayerEventId, 2)
                    .RuleFor(x => x.Minutes, 46)
            )
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(2);
    }

    [Fact]
    public void Should_award_0_points_if_user_has_unique_captain_but_played_less_than_46_minutes()
    {
        var opponent = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetCaptain().RuleFor(x => x.FantasyPlayerEventId, 1))
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetCaptain()
                    .RuleFor(x => x.FantasyPlayerEventId, 2)
                    .RuleFor(x => x.Minutes, 45)
            )
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_award_0_points_if_user_captain_is_not_unique()
    {
        var opponent = new ManagerGameWeekStateBuilder()
            .WithLineup(TestStateFactory.GetCaptain().RuleFor(x => x.FantasyPlayerEventId, 1))
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetCaptain()
                    .RuleFor(x => x.FantasyPlayerEventId, 1)
                    .RuleFor(r => r.Minutes, 46)
            )
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
