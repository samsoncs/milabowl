using FluentAssertions;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class YellowCardsTests: MilaRuleTest<YellowCards>
{
    [Fact]
    public void Should_get_one_point_for_every_playing_player()
    {
        var gameWeekState = StateFactory
            .GetMilaGameWeekState(
                [
                    StateFactory.GetPlayer().RuleFor(r => r.YellowCards, 1),
                    StateFactory.GetCaptain().RuleFor(r => r.YellowCards, 1),
                    StateFactory.GetPlayer().RuleFor(r => r.YellowCards, 0),
                    StateFactory.GetBenchPlayer().RuleFor(r => r.YellowCards, 1),
                ]
            );

        var result = Rule.Calculate(
            gameWeekState
        );

        result.Points.Should().Be(3);
    }
}
