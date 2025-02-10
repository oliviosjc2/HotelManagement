using HM.Domain.Entities;
using HM.Domain.Entities.Identity;
using HM.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelEntity = HM.Domain.Entities.Hotel;

namespace HM.Application.Events.Hotel
{
    public class HotelCreatedEvent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _uow;
        public HotelCreatedEvent(UserManager<ApplicationUser> userManager,
            IUnitOfWork uow)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(int hotelId, CancellationToken cancellationToken)
        {
            try
            {
                var hotelRepository = _uow.GetRepository<HotelEntity>();

                var hotel = await hotelRepository
                    .Get()
                    .Include(i => i.HotelAdmins)
                    .FirstOrDefaultAsync(f => f.Id == hotelId, cancellationToken);

                if(hotel is not null)
                {
                    var hotelAdminUser = new ApplicationUser
                    {
                        UserName = hotel.Email,
                        Email = hotel.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(hotelAdminUser, "Hotel@123");
                    if(result.Succeeded)
                        await _userManager.AddToRoleAsync(hotelAdminUser, "HOTEL_ADMIN");

                    var nHotelAdmin = new HotelAdmin
                    {
                        HotelAdminUser = hotelAdminUser,
                        Actived = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    hotel.HotelAdmins = new List<HotelAdmin>
                    {
                        nHotelAdmin
                    };

                    await hotelRepository.UpdateAsync(hotel, cancellationToken);
                    await _uow.CommitAsync(cancellationToken);
                    _uow.Dispose();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
