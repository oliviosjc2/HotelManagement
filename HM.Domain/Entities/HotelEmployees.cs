using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class HotelEmployees : BaseEntity
    {
        public int HotelId { get; set; } = default;
        public int EmployeeUserId { get; set; } = default;

        public virtual Hotel Hotel { get; set; } = new();
        public virtual ApplicationUser EmployeeUser { get; set; } = new();
    }
}
