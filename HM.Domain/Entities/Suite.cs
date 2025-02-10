using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class Suite : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PeopleCapacity { get; set; } = default;
        public int HotelId { get; set; } = default;
        public int SuiteCategoryId { get; set; } = default;
        public decimal DailyPriceDefault { get; set; } = default;
        public int HotelUserId { get; set; } = default;

        public virtual Hotel? Hotel { get; set; }
        public virtual SuiteCategory? SuiteCategory { get; set; }
        public virtual ICollection<SuitePhoto>? SuitePhotos { get; set; }
        public virtual ApplicationUser? HotelUser { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<Reserve>? Reserves { get; set; }
        public virtual ICollection<SuiteSchedule>? Schedules { get; set; }
    }
}
