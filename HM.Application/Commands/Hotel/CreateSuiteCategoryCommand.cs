using HM.Application.Response;
using MediatR;

namespace HM.Application.Commands.Hotel
{
    public class CreateSuiteCategoryCommand : IRequest<ResponseViewModel<string>>
    {
        public string Name { get; set; } = string.Empty;
        public int HotelId { get; set; } = default;
    }
}
