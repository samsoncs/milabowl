using Milabowl.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milabowl.Repositories
{
    public interface IMilaRepository
    {
        Task<IList<MilaGWScore>> GetMilaGwScores();
    }
}
