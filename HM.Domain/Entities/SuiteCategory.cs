using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class SuiteCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int HotelId { get; set; } = default;
        public int HotelUserId { get; set; } = default;
        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<Suite>? Suites { get; set; }
        public virtual ApplicationUser? HotelUser { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
    }
}
