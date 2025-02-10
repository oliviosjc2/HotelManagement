using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class SuitePhoto : BaseEntity
    {
        public int SuiteId { get; set; } = default;
        public string BucketURL { get; set; } = string.Empty;
        public int HotelUserId { get; set; } = default;

        public bool ItsMainPhoto { get; set; } = default;
        public virtual Suite Suite { get; set; } = new();
        public virtual ApplicationUser HotelUser { get; set; } = new();
    }
}
