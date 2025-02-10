using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;
using HM.Domain.Enumerator;

namespace HM.Domain.Entities
{
    public class Reserve : BaseEntity
    {
        public int SuiteId { get; set; } = default;
        public int HotelId { get; set; } = default;
        public DateTime StartDate { get; set; } = default;
        public DateTime EndDate { get; set; } = default;
        public int CustomerUserId { get; set; } = default;
        public int? InvoiceId { get; set; } = default;
        public bool Paid { get; set; } = default;
        public ReserveStatusEnumerator Status { get; set; } = default;
        public virtual ApplicationUser? CustomerUser { get; set; }
        public virtual List<ReserveCustomerInformations>? CustomerInformations { get; set; }
        public virtual Invoice? Invoice { get; set; }
        public virtual Suite? Suite { get; set; }
        public virtual Hotel? Hotel { get; set; }
    }
}
