using HM.Application.Commands.Hotel;
using HM.Application.Response;
using HM.Domain.Entities;
using HM.Domain.Repositories;
using HM.Infra.RequestContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using HotelEntity = HM.Domain.Entities.Hotel;

namespace HM.Application.Handlers.Hotel
{
    public class CreateSuiteCommandHandler
        : IRequestHandler<CreateSuiteCommand, ResponseViewModel<string>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IUnitOfWork _uow;

        public CreateSuiteCommandHandler(IUserContextService userContextService,
            IUnitOfWork uow)
        {
            _uow = uow;
            _userContextService = userContextService;   
        }

        public async Task<ResponseViewModel<string>> Handle(CreateSuiteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var suiteRepository = _uow.GetRepository<Suite>();
                var hotelRepository = _uow.GetRepository<HotelEntity>();
                var suiteCategoryRepository = _uow.GetRepository<SuiteCategory>();

                var hotel = await hotelRepository
                    .Get()
                    .AsNoTracking()
                    .Include(i => i.SuiteCategories)
                    .Include(i => i.HotelEmployees)
                    .Include(i => i.HotelAdmins)
                    .FirstOrDefaultAsync(f => f.Id == request.HotelId,
                    cancellationToken);

                if (hotel is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Hotel não encontrado na base de dados.");

                var hotelEmployee = hotel.HotelEmployees?.FirstOrDefault(f => f.EmployeeUserId == _userContextService.GetUserId());
                var hotelAdmin = hotel.HotelAdmins?.FirstOrDefault(f => f.HotelAdminUserId == _userContextService.GetUserId());

                var userId = hotelEmployee?.EmployeeUserId ?? hotelAdmin?.HotelAdminUserId;

                if (userId is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Sem permissão!");

                var suiteCategory = await suiteCategoryRepository
                    .Get()
                    .AsNoTracking()
                    .Include(i => i.Suites)
                    .FirstOrDefaultAsync(f => f.Id == request.SuiteCategoryId, cancellationToken);

                if (suiteCategory is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Categoria de Suíte não encontrada na base de dados.");

                var hotelSuiteCategory = hotel.SuiteCategories?.FirstOrDefault(f => f.Id == request.SuiteCategoryId);
                if (hotelSuiteCategory is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Categoria de Suíte inválida para o hotel.");

                var nSuite = new Suite
                {
                    Name = request.Name,
                    Actived = true,
                    CreatedAt = DateTime.UtcNow,
                    DailyPriceDefault = request.DailyPriceDefault,
                    Description = request.Description,
                    HotelId = hotel.Id,
                    HotelUserId = userId.Value,
                    PeopleCapacity = request.PeopleCapacity,
                    SuiteCategoryId = suiteCategory.Id
                };

                await suiteRepository.AddAsync(nSuite, cancellationToken);
                await _uow.CommitAsync(cancellationToken);

                return ResponseViewModel<string>.GetResponse(HttpStatusCode.OK, "Suite cadastrada com sucesso!");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                _uow.Dispose();
            }
        }
    }
}
