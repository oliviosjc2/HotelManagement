using FluentValidation;
using Hangfire;
using HM.Application.Commands.Hotel;
using HM.Application.Events.Hotel;
using HM.Application.Response;
using HM.Domain.Entities.Identity;
using HM.Domain.Repositories;
using HM.Infra.RequestContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using HotelEntity = HM.Domain.Entities.Hotel;

namespace HM.Application.Handlers.Hotel
{
    public class CreateHotelCommandHandler
        : IRequestHandler<CreateHotelCommand, ResponseViewModel<string>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateHotelCommand> _validator;
        private readonly IUserContextService _userContextService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateHotelCommandHandler(IUnitOfWork uow,
            IValidator<CreateHotelCommand> validator,
            IUserContextService userContextService,
            UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _validator = validator;
            _userManager = userManager;
            _userContextService = userContextService;
        }

        public async Task<ResponseViewModel<string>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return ResponseViewModel<string>.GetResponse(validationResult);

                var hotelAdminUser = await _userManager.FindByEmailAsync(request.Email);

                if (hotelAdminUser is not null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, 
                        "O e-mail informado já se encontra em uso.");

                var hotelRepository = _uow.GetRepository<HotelEntity>();

                var hotel = await hotelRepository
                    .Get()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Name.ToUpper() == request.Name.ToUpper() 
                    || f.Email.ToUpper() == request.Email.ToUpper(), cancellationToken);

                if (hotel is not null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, 
                        "Já existe um hotel com este nome e/ou e-mail.");

                var currentUser = await _userManager.FindByIdAsync(_userContextService.GetUserId().ToString());

                hotel = new HotelEntity(request.Name, request.Email,
                    request.AcceptMinors, request.BookingConfirmationTimeInMinutes, currentUser);

                await hotelRepository.AddAsync(hotel, cancellationToken);
                await _uow.CommitAsync(cancellationToken);

                BackgroundJob.Enqueue<HotelCreatedEvent>(job => job.ExecuteAsync(hotel.Id, cancellationToken));
                return ResponseViewModel<string>.GetResponse(HttpStatusCode.OK,
                    "Hotel cadastrado com sucesso na base de dados!");
            }
            catch (Exception)
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
