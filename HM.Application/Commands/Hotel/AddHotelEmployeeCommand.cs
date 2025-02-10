using HM.Application.Response;
using MediatR;

namespace HM.Application.Commands.Hotel
{
    public class AddHotelEmployeeCommand : IRequest<ResponseViewModel<string>>
    {
        public int HotelId { get; set; }
        public string EmployeeEmail { get; set; } = string.Empty;
    }
}
