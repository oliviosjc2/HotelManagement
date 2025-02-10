using HM.Application.Response;
using MediatR;

namespace HM.Application.Commands.Hotel
{
    public class CreateReserveCommand : IRequest<ResponseViewModel<string>>
    {
        public int SuiteId { get; set; } = default;
        public DateTime StartDate { get; set; } = default;
        public DateTime EndDate { get; set; } = default;
        public List<ReserveCustomersCommand> Customers { get; set; } = new();
    }

    public class ReserveCustomersCommand
    {
        public string Fullname { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; } = default;
        public string? CustomerEmail { get; set; }
        public string? CustomerCellphone { get; set; }
    }
}
