using HM.Application.Helpers;
using HM.Application.Queries;
using HM.Application.Response;
using HM.Domain.Entities;
using HM.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace HM.Application.Handlers.Hotel
{
    public class GetInvoicesDetailsQueryHandler
        : IRequestHandler<GetInvoicesDetailsQuery, ResponseViewModel<List<Invoice>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemoryCache _memoryCache;

        public GetInvoicesDetailsQueryHandler(IUnitOfWork uow,
            IMemoryCache memoryCache)
        {
            _uow = uow;
            _memoryCache = memoryCache;
        }

        public async Task<ResponseViewModel<List<Invoice>>> Handle(GetInvoicesDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var invoiceRepository = _uow.GetRepository<Invoice>();

                string cacheKey = $"invoices_{request.Date:yyyy_MM}";

                if (!_memoryCache.TryGetValue(cacheKey, out List<Invoice> invoices))
                {
                    invoices = await invoiceRepository
                        .Get()
                        .AsNoTracking()
                        .Where(wh => wh.PaymentDate.HasValue
                                  && wh.PaymentDate.Value.Year == request.Date.Year
                                  && wh.PaymentDate.Value.Month == request.Date.Month)
                        .ToListAsync(cancellationToken);

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };

                    _memoryCache.Set(cacheKey, invoices, cacheOptions);
                }

                if(!invoices.AnySafe())
                    return ResponseViewModel<List<Invoice>>.GetResponse(HttpStatusCode.NoContent, "Nenhum faturamento encontrado para este mês/ano!");

                return ResponseViewModel<List<Invoice>>.GetResponse(HttpStatusCode.OK, "Faturamento encontrado com sucesso!");
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
