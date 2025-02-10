using HM.Domain.Entities;
using HM.Domain.Enumerator;

namespace HM.Application.HandlersResponse
{
    public class GetAllReservesQueryHandlerResponse
    {
        public GetAllReservesQueryHandlerResponse()
        {
            
        }

        public GetAllReservesQueryHandlerResponse(Reserve reserve)
        {
            ReserveId = reserve.Id;
            SuiteId = reserve.SuiteId;
            HotelId = reserve.HotelId;
            Status = reserve.Status;
        }

        public int ReserveId { get; set; } = default;
        public int SuiteId { get; set; } = default;
        public int HotelId { get; set; } = default;
        public ReserveStatusEnumerator Status { get; set; } = default;
    }
}
