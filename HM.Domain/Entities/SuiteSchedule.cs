using HM.Domain.Entities.Base;

namespace HM.Domain.Entities
{
    public class SuiteSchedule : BaseEntity
    {
        public int SuiteId { get; set; } = default;
        public DateTime Date { get; set; } = default;
        public virtual Suite? Suite { get; set; }
    }
}
