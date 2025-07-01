using Milabowl.Processing.Processing;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.ConventionTests;

public class AllMilaRulesShouldHaveTest
{
    [Fact]
    public void All_MilaRules_Should_Have_Test()
    {
        // Arrange
        var milaRules = typeof(IMilaRule)
            .Assembly.GetTypes()
            .Where(t => t.IsClass && typeof(IMilaRule).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => t.Name)
            .OrderBy(t => t)
            .ToList();

        var milaRuleTests = typeof(MilaRuleTest<>)
            .Assembly.GetTypes()
            .Where(t =>
                t.BaseType is not null
                && t.BaseType.IsGenericType
                && t.BaseType.GetGenericTypeDefinition() == typeof(MilaRuleTest<>)
            )
            .Select(t => t.BaseType!.GetGenericArguments()[0].Name)
            .OrderBy(t => t)
            .ToList();

        milaRuleTests.ShouldBeEquivalentTo(
            milaRules,
            $"all MilaRules should have a corresponding test class implementing MilaRuleTest<T> where T is the MilaRule type. Missing: {string.Join(",", milaRules.Where(r => !milaRuleTests.Contains(r)))}"
        );
    }
}
