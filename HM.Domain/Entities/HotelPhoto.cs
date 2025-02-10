using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class HotelPhoto : BaseEntity
    {
        public int HotelId { get; set; } = default;
        public string BucketURL { get; set; } = string.Empty;
        public bool ItsMainPhoto { get; set; } = default;
        public int HotelUserId { get; set; } = default;
        public virtual Hotel Hotel { get; set; } = new();
        public virtual ApplicationUser HotelUser {  get; set; } = new();
    }
}
