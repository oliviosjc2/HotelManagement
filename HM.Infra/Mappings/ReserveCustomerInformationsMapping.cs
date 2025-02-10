using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class ReserveCustomerInformationsMapping : IEntityTypeConfiguration<ReserveCustomerInformations>
    {
        public void Configure(EntityTypeBuilder<ReserveCustomerInformations> entity)
        {
            entity.ToTable("ReserveCustomerInformations");

            entity.Property(p => p.CustomerFullname).HasMaxLength(255).IsRequired();
            entity.Property(p => p.CustomerBirthdayDate).IsRequired();
            entity.Property(p => p.CustomerEmail).HasMaxLength(64);
            entity.Property(p => p.CustomerCellphone).HasMaxLength(13);

            entity.HasOne(e => e.Reserve)
                .WithMany(r => r.CustomerInformations)
                .HasForeignKey(f => f.ReserveId);

        }
    }
}
