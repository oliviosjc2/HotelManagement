using HM.Application.Commands.Hotel;
using HM.Application.Events.Hotel;
using HM.Application.Response;
using HM.Domain.Entities;
using HM.Domain.Enumerator;
using HM.Domain.Repositories;
using HM.Infra.RequestContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HM.Application.Handlers.Hotel
{
    public class CreateReserveCommandHandler
        : IRequestHandler<CreateReserveCommand, ResponseViewModel<string>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserContextService _userContextService;
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _suiteLocks = new();

        public CreateReserveCommandHandler(IUnitOfWork uow,
            IUserContextService userContextService)
        {
            _uow = uow;
            _userContextService = userContextService;
        }

        public async Task<ResponseViewModel<string>> Handle(CreateReserveCommand request, CancellationToken cancellationToken)
        {
            var semaphore = _suiteLocks.GetOrAdd(request.SuiteId, _ => new SemaphoreSlim(1, 1));
            try
            {
                await semaphore.WaitAsync(cancellationToken);

                var suiteRepository = _uow.GetRepository<Suite>();
                var reserveRepository = _uow.GetRepository<Reserve>();

                var suite = await suiteRepository
                    .Get()
                    .Include(i => i.Hotel)
                    .Include(i => i.Schedules)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == request.SuiteId, cancellationToken);

                if (suite is null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Suite não encontrada na base de dados.");

                var dateRange = Enumerable.Range(0, (request.EndDate - request.StartDate).Days + 1)
                          .Select(offset => request.StartDate.AddDays(offset))
                          .ToArray();

                var hasConflict = suite.Schedules?.Any(schedule => dateRange.Contains(schedule.Date.Date)) ?? false;

                if (hasConflict)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "A suíte já está reservada para a sua data.");

                if (request.Customers.Count + 1 > suite.PeopleCapacity)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, $"O número máximo de pessoas na suíte é de {suite.PeopleCapacity}!");

                var customers = new List<ReserveCustomerInformations>();

                foreach (var customer in request.Customers)
                {
                    if (IsMinor(customer.Birthdate) && !suite.Hotel!.AcceptMinors)
                        return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, $"O cliente {customer.Fullname} é menor de idade.");

                    var nCustomer = new ReserveCustomerInformations
                    {
                        Actived = true,
                        CreatedAt = DateTime.UtcNow,
                        CustomerBirthdayDate = customer.Birthdate,
                        CustomerCellphone = customer.CustomerCellphone,
                        CustomerEmail = customer.CustomerEmail,
                        CustomerFullname = customer.Fullname
                    };

                    customers.Add(nCustomer);
                }

                var reserve = new Reserve
                {
                    CreatedAt = DateTime.UtcNow,
                    Actived = true,
                    CustomerUserId = _userContextService.GetUserId(),
                    EndDate = request.EndDate.Date,
                    StartDate = request.StartDate.Date,
                    Paid = false,
                    SuiteId = suite.Id,
                    CustomerInformations = customers,
                    Status = ReserveStatusEnumerator.WAITING_PAYMENT,
                    HotelId = suite.HotelId
                };

                if (suite.Schedules is null)
                    suite.Schedules = new List<SuiteSchedule>();

                foreach(var date in dateRange)
                {
                    suite.Schedules.Add(new SuiteSchedule 
                    { 
                        Actived = true, Date = date
                    });
                }

                await reserveRepository.AddAsync(reserve, cancellationToken);
                await suiteRepository.UpdateAsync(suite, cancellationToken);

                await _uow.CommitAsync(cancellationToken);

                Hangfire.BackgroundJob
                    .Schedule<UpdateReserveNoPaidEvent>((job) => 
                    job.ExecuteAsync(reserve.Id, cancellationToken), 
                    DateTime.UtcNow.AddMinutes(suite.Hotel!.BookingConfirmationTimeInMinutes));

                Hangfire.BackgroundJob.Enqueue<ReserveCreatedEvent>((job) => job.ExecuteAsync(reserve.Id, cancellationToken));

                return ResponseViewModel<string>.GetResponse(HttpStatusCode.OK, "Reserva efetuada com sucesso. Realize o pagamento em até 15minutos.");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();
                _uow.Dispose();
            }
        }

        private static bool IsMinor(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age < 18;
        }
    }
}
