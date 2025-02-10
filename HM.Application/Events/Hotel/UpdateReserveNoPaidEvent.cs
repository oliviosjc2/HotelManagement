using HM.Domain.Entities;
using HM.Domain.Enumerator;
using HM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HM.Application.Events.Hotel
{
    public class UpdateReserveNoPaidEvent
    {
        private readonly IUnitOfWork _uow;

        public UpdateReserveNoPaidEvent(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task ExecuteAsync(int reserveId, CancellationToken cancellationToken)
        {
            try
            {
                var reserveRepository = _uow.GetRepository<Reserve>();
                var invoiceRepository = _uow.GetRepository<Invoice>();
                var suiteScheduleRepository = _uow.GetRepository<SuiteSchedule>();

                var reserve = await reserveRepository
                    .Get()
                    .Include(i => i.Invoice)
                    .FirstOrDefaultAsync(f => f.Id == reserveId, cancellationToken);

                if (reserve is null)
                    throw new Exception($"Reserva não encontrada para o Id {reserveId}!");

                reserve.Status = ReserveStatusEnumerator.CANCELLED;

                if (reserve.Invoice is not null)
                    await invoiceRepository.DeleteAsync(reserve.Invoice, cancellationToken);

                var suiteSchedules = await suiteScheduleRepository
                    .Get()
                    .Where(wh => wh.SuiteId == reserve.SuiteId
                              && wh.Date >= reserve.StartDate
                              && wh.Date <= reserve.EndDate)
                    .ToListAsync(cancellationToken);

                await suiteScheduleRepository.DeleteAsync(suiteSchedules, cancellationToken);
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
