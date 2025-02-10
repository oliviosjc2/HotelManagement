using HM.Application.HandlersResponse;
using HM.Application.Helpers;
using HM.Application.Queries;
using HM.Application.Response;
using HM.Infra.Cache.Interfaces;
using MediatR;
using System.Net;

namespace HM.Application.Handlers.Hotel
{
    public class GetAllReservesQueryHandler
        : IRequestHandler<GetAllReservesQuery, ResponseViewModel<List<GetAllReservesQueryHandlerResponse>>>
    {
        private readonly IReserveCacheService _reserveCacheService;

        public GetAllReservesQueryHandler(IReserveCacheService reserveCacheService)
        {
            _reserveCacheService = reserveCacheService;
        }

        public async Task<ResponseViewModel<List<GetAllReservesQueryHandlerResponse>>> Handle(GetAllReservesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reservesEntity = await _reserveCacheService.GetReservesAsync(request.StartDate, request.EndDate,
                    request.HotelId, request.SuiteId);

                if (reservesEntity?.AnySafe() is not true)
                    return ResponseViewModel<List<GetAllReservesQueryHandlerResponse>>.GetResponse(HttpStatusCode.NoContent,
                        "Nenhuma reserva foi encontrada na base de dados.");

                var response = reservesEntity.Select(sl => new GetAllReservesQueryHandlerResponse(sl)).ToList();

                return ResponseViewModel<List<GetAllReservesQueryHandlerResponse>>.GetResponse(HttpStatusCode.OK,
                                        "Reservas encontradas na base de dados.", response);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
