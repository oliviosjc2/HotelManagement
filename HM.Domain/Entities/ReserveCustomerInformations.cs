using HM.Domain.Entities.Base;

namespace HM.Domain.Entities
{
    public class ReserveCustomerInformations : BaseEntity
    {
        public int ReserveId { get; set; } = default;
        public string CustomerFullname { get; set; } = string.Empty;
        public DateTime CustomerBirthdayDate { get; set; } = default;
        public string? CustomerEmail { get; set; }
        public string? CustomerCellphone { get; set; }
        public virtual Reserve? Reserve { get; set; }
    }
}
