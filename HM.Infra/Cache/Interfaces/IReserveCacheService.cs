using HM.Domain.Entities;

namespace HM.Infra.Cache.Interfaces
{
    public interface IReserveCacheService
    {
        Task<List<Reserve>?> GetReservesAsync(DateTime? startDate, DateTime? endDate, int? hotelId, int? suiteId);
    }
}
