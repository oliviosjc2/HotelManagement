using HM.Domain.Entities.Base;
using HM.Domain.Enumerator;

namespace HM.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public int HotelId { get; set; } = default;
        public int SuiteId { get; set; } = default;
        public int SuiteCategoryId { get; set; } = default;
        public int ReserveId { get; set; } = default;
        public decimal Value { get; set; } = default;
        public DateTime? PaymentDate { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public bool Paid { get; set; } = default;
        public PaymentMethodEnumerator? PaymentMethod { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual Suite? Suite { get; set; }
        public virtual SuiteCategory? SuiteCategory { get; set; }
        public virtual Reserve? Reserve { get; set; }
    }
}
