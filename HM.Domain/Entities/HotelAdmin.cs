using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class HotelAdmin : BaseEntity
    {
        public int HotelId { get; set; } = default;
        public int HotelAdminUserId { get; set; } = default;

        public virtual Hotel Hotel { get; set; } = new();
        public virtual ApplicationUser HotelAdminUser { get; set; } = new();
    }
}
