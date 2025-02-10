using HM.Application.Response;
using HM.Domain.Entities;
using MediatR;

namespace HM.Application.Queries
{
    public class GetInvoicesDetailsQuery
        : IRequest<ResponseViewModel<List<Invoice>>>
    {
        public DateTime Date { get; set; } = default;
    }
}
