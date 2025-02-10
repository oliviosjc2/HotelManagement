using HM.Domain.Entities;
using HM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HM.Application.Events.Hotel
{
    public class ReserveCreatedEvent
    {
        private readonly IUnitOfWork _uow;

        public ReserveCreatedEvent(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task ExecuteAsync(int reserveId, CancellationToken cancellationToken)
        {
            try
            {
                var reserveRepository = _uow.GetRepository<Reserve>();
                var invoiceRepository = _uow.GetRepository<Invoice>();

                var reserve = await reserveRepository
                    .Get()
                    .Include(i => i.Hotel)
                    .Include(i => i.CustomerInformations)
                    .FirstOrDefaultAsync(f => f.Id == reserveId, cancellationToken);

                if (reserve is null)
                    throw new Exception($"A reserva não foi encontrada com o Id {reserveId}.");

                var qtdDays = reserve.EndDate.Date - reserve.StartDate.Date;

                var invoice = new Invoice
                {
                    Actived = true,
                    CreatedAt = DateTime.UtcNow,
                    HotelId = reserve.HotelId,
                    SuiteId = reserve.SuiteId,
                    Paid = false,
                    PaymentDeadline = DateTime.UtcNow.AddMinutes(reserve.Hotel!.BookingConfirmationTimeInMinutes),
                    PaymentMethod = null,
                    PaymentDate = null,
                    Value = qtdDays.Days * reserve.Suite!.DailyPriceDefault,
                    SuiteCategoryId = reserve.Suite!.SuiteCategoryId
                };

                await invoiceRepository.AddAsync(invoice, cancellationToken);
                await _uow.CommitAsync(cancellationToken);

                _uow.Dispose();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}