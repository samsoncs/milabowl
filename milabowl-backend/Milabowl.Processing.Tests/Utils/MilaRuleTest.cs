using Milabowl.Processing.Processing;

namespace Milabowl.Processing.Tests.Utils;

public abstract class MilaRuleTest<T>
    where T : IMilaRule
{
    protected IMilaRule Rule => Activator.CreateInstance<T>();
}
