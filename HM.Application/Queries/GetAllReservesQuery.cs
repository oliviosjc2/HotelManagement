using HM.Application.HandlersResponse;
using HM.Application.Response;
using MediatR;

namespace HM.Application.Queries
{
    public class GetAllReservesQuery : IRequest<ResponseViewModel<List<GetAllReservesQueryHandlerResponse>>>
    {
        public int? HotelId { get; set; }
        public int? SuiteId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
