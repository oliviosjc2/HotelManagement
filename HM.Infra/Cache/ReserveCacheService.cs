using HM.Domain.Entities;
using HM.Domain.Repositories;
using HM.Infra.Cache.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HM.Infra.Cache
{
    public class ReserveCacheService : IReserveCacheService
    {
        private readonly IUnitOfWork _uow;
        private readonly IDistributedCache _cache;

        public ReserveCacheService(IUnitOfWork uow,
            IDistributedCache cache)
        {
            _uow = uow;
            _cache = cache;
        }

        public async Task<List<Reserve>?> GetReservesAsync(DateTime? startDate, DateTime? endDate, int? hotelId, int? suiteId)
        {
            try
            {
                string cacheKey = $"Reserves:{hotelId}-{suiteId}-{startDate?.ToString("yyyy-MM-dd")}-{endDate?.ToString("yyyy-MM-dd")}";
                var cachedReserves = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedReserves))
                {
                    var reserves = JsonSerializer.Deserialize<List<Reserve>>(cachedReserves);
                    return reserves;
                }

                var reserveRepository = _uow.GetRepository<Reserve>();
                var query = reserveRepository.Get();

                if (hotelId.HasValue)
                    query = query.Where(wh => wh.HotelId == hotelId.Value);

                if (suiteId.HasValue)
                    query = query.Where(wh => wh.SuiteId == suiteId.Value);

                if (startDate.HasValue)
                    query = query.Where(r => r.StartDate >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(r => r.EndDate <= endDate.Value);

                var reservesList = await query.ToListAsync();

                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(reservesList), options);
                return reservesList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
