using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Milabowl.Business.Api;
using Milabowl.Business.DTOs.Api;
using Milabowl.Utils;

namespace Milabowl.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MilaResultsController: ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IMilaResultsService _milaResultsService;

        public MilaResultsController(IMemoryCache cache, IMilaResultsService milaResultsService)
        {
            this._cache = cache;
            this._milaResultsService = milaResultsService;
        }

        [HttpGet]
        public async Task<MilaResultsDTO> GetMilaResults()
        {
            if (this._cache.TryGetValue(CacheKeys.MilaResults, out MilaResultsDTO cachedMilaResults))
            {
                return cachedMilaResults;
            }

            cachedMilaResults = await this._milaResultsService.GetMilaResults();
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7));
            this._cache.Set(CacheKeys.MilaResults, cachedMilaResults, cacheOptions);

            return cachedMilaResults;
        }

        [HttpGet("clear-cache")]
        public void ClearCache()
        {
            this._cache.Remove(CacheKeys.MilaResults);
        }
    }
}
