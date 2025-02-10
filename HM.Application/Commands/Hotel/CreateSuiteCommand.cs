using HM.Application.Response;
using MediatR;

namespace HM.Application.Commands.Hotel
{
    public class CreateSuiteCommand
        : IRequest<ResponseViewModel<string>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PeopleCapacity { get; set; } = default;
        public int SuiteCategoryId { get; set; } = default;
        public int HotelId { get; set; } = default;
        public decimal DailyPriceDefault { get; set; } = default;
    }
}
