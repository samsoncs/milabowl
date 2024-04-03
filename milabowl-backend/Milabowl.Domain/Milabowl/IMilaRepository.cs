using Milabowl.Domain.Entities.Milabowl;

namespace Milabowl.Domain.Milabowl;

public interface IMilaRepository
{
    Task<IList<MilaGWScore>> GetMilaGwScores();
    Task<FplResults> GetFplResults();
}