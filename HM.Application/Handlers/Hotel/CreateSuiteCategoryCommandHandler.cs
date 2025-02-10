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
    public class CreateSuiteCategoryCommandHandler
        : IRequestHandler<CreateSuiteCategoryCommand, ResponseViewModel<string>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IUnitOfWork _uow;

        public CreateSuiteCategoryCommandHandler(IUserContextService userContextService,
            IUnitOfWork uow)
        {
            _uow = uow;
            _userContextService = userContextService;
        }

        public async Task<ResponseViewModel<string>> Handle(CreateSuiteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotelRepository = _uow.GetRepository<HotelEntity>();
                var hotelAdminRepository = _uow.GetRepository<HotelAdmin>();
                var suiteCategoryRepository = _uow.GetRepository<SuiteCategory>();

                var hotel = await hotelRepository
                    .Get()
                    .Include(i => i.SuiteCategories)
                    .FirstOrDefaultAsync(f => f.Id == request.HotelId,
                    cancellationToken);

                if (hotel is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Hotel não encontrado na base de dados.");

                var hotelAdminValid = await hotelAdminRepository
                    .Get()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.HotelId == request.HotelId && f.HotelAdminUserId == _userContextService.GetUserId(),
                    cancellationToken);

                if (hotelAdminValid == null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.Forbidden, "Sem permissão!");

                var suiteCategory = await suiteCategoryRepository
                    .Get()
                    .FirstOrDefaultAsync(f => f.Name.ToUpper() == request.Name.ToUpper() 
                                           && f.HotelId == request.HotelId, 
                    cancellationToken);

                if (suiteCategory is not null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, 
                        "Já existe uma categoria de Suíte com este nome para este Hotel.");

                suiteCategory = new SuiteCategory
                {
                    Name = request.Name.ToUpper(),
                    Actived = true,
                    CreatedAt = DateTime.UtcNow,
                    HotelUserId = _userContextService.GetUserId()
                };

                if (hotel.SuiteCategories is null)
                    hotel.SuiteCategories = new List<SuiteCategory>
                    { suiteCategory };
                else
                    hotel.SuiteCategories.Add(suiteCategory);

                await hotelRepository.UpdateAsync(hotel, cancellationToken);
                await _uow.CommitAsync(cancellationToken);

                return ResponseViewModel<string>.GetResponse(HttpStatusCode.OK, "Categoria de Suíte cadastrada com sucesso para o hotel.");
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                _uow.Dispose();
            }
        }
    }
}
