using HM.Application.Commands.Hotel;
using HM.Application.Response;
using HM.Domain.Entities;
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
    public class AddHotelEmployeeCommandHandler
        : IRequestHandler<AddHotelEmployeeCommand, ResponseViewModel<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserContextService _userContextService;
        private readonly IUnitOfWork _uow;

        public AddHotelEmployeeCommandHandler(UserManager<ApplicationUser> userManager,
            IUserContextService userContextService,
            IUnitOfWork uow)
        {
            _userContextService = userContextService;
            _userManager = userManager;
            _uow = uow;
        }

        public async Task<ResponseViewModel<string>> Handle(AddHotelEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotelAdminValid = await ValidateHotelAdminAsync(request.HotelId);
                if (hotelAdminValid is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.Forbidden, "Sem permissões!");

                var hotel = await GetHotelAsync(request.HotelId, cancellationToken);
                if (hotel is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Hotel não encontrado na base de dados!");

                var employeeUser = await GetOrCreateEmployeeUserAsync(request.EmployeeEmail);
                if (employeeUser is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Erro ao criar o usuário funcionário.");

                var hotelEmployeeExists = await CheckIfEmployeeAlreadyLinkedToHotelAsync(employeeUser.Id, request.HotelId, cancellationToken);
                if (hotelEmployeeExists)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "O funcionário já está vinculado a este hotel.");

                await LinkEmployeeToHotelAsync(hotel, employeeUser);

                await _uow.CommitAsync(cancellationToken);

                return ResponseViewModel<string>.GetResponse(HttpStatusCode.OK, "Funcionário vinculado com sucesso ao hotel.");
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

        private async Task<HotelAdmin?> ValidateHotelAdminAsync(int hotelId)
        {
            var hotelAdminRepository = _uow.GetRepository<HotelAdmin>();
            return await hotelAdminRepository
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.HotelAdminUserId == _userContextService.GetUserId()
                                           && f.HotelId == hotelId);
        }

        private async Task<HotelEntity?> GetHotelAsync(int hotelId, CancellationToken cancellationToken)
        {
            var hotelRepository = _uow.GetRepository<HotelEntity>();
            return await hotelRepository
                .Get()
                .Include(i => i.HotelEmployees)
                .FirstOrDefaultAsync(f => f.Id == hotelId, cancellationToken);
        }

        private async Task<ApplicationUser> GetOrCreateEmployeeUserAsync(string employeeEmail)
        {
            var employeeUser = await _userManager.FindByEmailAsync(employeeEmail);
            if (employeeUser is null)
            {
                employeeUser = new ApplicationUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(employeeUser, "Employee@123");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(employeeUser, "HOTEL_EMPLOYEE");
                else
                    return null;
            }
            return employeeUser;
        }

        private async Task<bool> CheckIfEmployeeAlreadyLinkedToHotelAsync(int employeeUserId, int hotelId, CancellationToken cancellationToken)
        {
            var hotelEmployeeRepository = _uow.GetRepository<HotelEmployees>();
            var hotelEmployee = await hotelEmployeeRepository
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.EmployeeUserId == employeeUserId && f.HotelId == hotelId, cancellationToken);
            return hotelEmployee != null;
        }

        private async Task LinkEmployeeToHotelAsync(HotelEntity hotel, ApplicationUser employeeUser)
        {
            var hotelEmployeeRepository = _uow.GetRepository<HotelEmployees>();
            var hotelEmployee = new HotelEmployees { EmployeeUser = employeeUser };

            if (hotel.HotelEmployees is null)
                hotel.HotelEmployees = new List<HotelEmployees> { hotelEmployee };
            else
                hotel.HotelEmployees.Add(hotelEmployee);

            var hotelRepository = _uow.GetRepository<HotelEntity>();
            await hotelRepository.UpdateAsync(hotel, CancellationToken.None);
        }
    }
}
