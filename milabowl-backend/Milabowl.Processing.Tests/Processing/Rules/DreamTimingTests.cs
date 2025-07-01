using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class DreamTimingTests : MilaRuleTest<DreamTiming>
{
    [Fact]
    public void Should_get_1point5_points_if_sub_in_dream_team()
    {
        var playerId = 1;
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(r => r.FantasyPlayerEventId, playerId)
                    .RuleFor(r => r.InDreamteam, true)
            )
            .WithSubsIn(TestStateFactory.GetSub().RuleFor(r => r.FantasyPlayerEventId, playerId))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(1.5m);
    }

    [Fact]
    public void Should_get_1point5_points_if_multiple_sub_in_dream_team()
    {
        var player1Id = 1;
        var player2Id = 1;
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(r => r.FantasyPlayerEventId, player1Id)
                    .RuleFor(r => r.InDreamteam, true),
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(r => r.FantasyPlayerEventId, player2Id)
                    .RuleFor(r => r.InDreamteam, true)
            )
            .WithSubsIn(
                TestStateFactory.GetSub().RuleFor(r => r.FantasyPlayerEventId, player1Id),
                TestStateFactory.GetSub().RuleFor(r => r.FantasyPlayerEventId, player2Id)
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(1.5m);
    }

    [Fact]
    public void Should_get_no_points_if_not_in_dream_team()
    {
        var playerId = 1;
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(r => r.FantasyPlayerEventId, playerId)
                    .RuleFor(r => r.InDreamteam, false)
            )
            .WithSubsIn(TestStateFactory.GetSub().RuleFor(r => r.FantasyPlayerEventId, playerId))
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
