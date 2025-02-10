using HM.Application.Response;
using MediatR;

namespace HM.Application.Commands.Hotel
{
    public class CreateHotelCommand : IRequest<ResponseViewModel<string>>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool AcceptMinors { get; set; }
        public int BookingConfirmationTimeInMinutes { get; set; } = default;
    }
}
