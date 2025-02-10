using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class SuiteScheduleMapping
        : IEntityTypeConfiguration<SuiteSchedule>
    {
        public void Configure(EntityTypeBuilder<SuiteSchedule> entity)
        {
            entity.ToTable("SuiteSchedules");

            entity.Property(p => p.Date).IsRequired();

            entity.HasOne(e => e.Suite)
                .WithMany(s => s.Schedules)
                .HasForeignKey(f => f.SuiteId);
        }
    }
}
