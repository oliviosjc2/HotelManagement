using HM.Domain.Entities;
using HM.Domain.Enumerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class ReserveMapping : IEntityTypeConfiguration<Reserve>
    {
        public void Configure(EntityTypeBuilder<Reserve> entity)
        {
            entity.ToTable("Reserves");

            entity.Property(p => p.StartDate).IsRequired();
            entity.Property(p => p.EndDate).IsRequired();
            entity.Property(p => p.Paid).HasDefaultValue(false).IsRequired();
            entity.Property(p => p.Status).HasDefaultValue(ReserveStatusEnumerator.WAITING_PAYMENT).IsRequired();

            entity.HasOne(e => e.Suite)
                .WithMany(s => s.Reserves)
                .HasForeignKey(f => f.SuiteId);

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.Reserves)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.CustomerUser)
                .WithMany(cu => cu.Reserves)
                .HasForeignKey(f => f.CustomerUserId);

            entity.HasOne(e => e.Invoice)
                .WithOne(i => i.Reserve)
                .HasForeignKey<Reserve>(r => r.InvoiceId);
        }
    }
}
