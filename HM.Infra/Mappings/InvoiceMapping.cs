using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HM.Infra.Mappings
{
    public class InvoiceMapping : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> entity)
        {
            entity.ToTable("Invoices");

            entity.Property(p => p.Value).IsRequired();
            entity.Property(p => p.PaymentDeadline).IsRequired();
            entity.Property(p => p.PaymentMethod).IsRequired();
            entity.Property(p => p.Paid).HasDefaultValue(false).IsRequired();

            entity.HasIndex(i => i.PaymentDate);
            entity.HasIndex(i => i.Paid);
            entity.HasIndex(i => i.HotelId);
            entity.HasIndex(i => i.SuiteId);

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.Invoices)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.Suite)
                .WithMany(s => s.Invoices)
                .HasForeignKey(f => f.SuiteId);

            entity.HasOne(e => e.SuiteCategory)
                .WithMany(sc => sc.Invoices)
                .HasForeignKey(f => f.SuiteCategoryId);

            entity.HasOne(e => e.Reserve)
                .WithOne(r => r.Invoice)
                .HasForeignKey<Invoice>(i => i.ReserveId);
        }
    }
}
