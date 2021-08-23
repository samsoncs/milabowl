using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Milabowl.Business.Import;
using Milabowl.Utils;

namespace Milabowl.Controllers
{
    [Authorize(Roles = ApplicationRoles.MilaAdmin)]
    [ApiController]
    [Route("api/[controller]")]
    public class DataImportController: ControllerBase
    {
        private readonly IDataImportService _dataImportService;
        private readonly IMilaPointsProcessorService _milaPointsProcessorService;
        private readonly IMemoryCache _cache;

        public DataImportController(IDataImportService dataImportService, IMilaPointsProcessorService _milaPointsProcessorService, IMemoryCache cache)
        {
            this._dataImportService = dataImportService;
            this._milaPointsProcessorService = _milaPointsProcessorService;
            this._cache = cache;
        }

        [HttpGet]
        public async Task ImportData()
        {
            await this._dataImportService.ImportData();
            this._cache.Remove(CacheKeys.MilaResults);
        }

        [HttpGet("process")]
        public async Task ProcessData()
        {
            await this._milaPointsProcessorService.UpdateMilaPoints();
            this._cache.Remove(CacheKeys.MilaResults);
        }

    }
}
