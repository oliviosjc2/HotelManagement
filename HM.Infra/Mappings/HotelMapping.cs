using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class HotelMapping : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> entity)
        {
            entity.ToTable("Hotels");

            entity.Property(p => p.Name).HasMaxLength(64).IsRequired();

            entity.Property(p => p.Email).HasMaxLength(255).IsRequired();

            entity.Property(p => p.AcceptMinors).HasDefaultValue(false).IsRequired();

            entity.Property(p => p.BookingConfirmationTimeInMinutes).HasDefaultValue(15).IsRequired();

            entity.HasOne(e => e.AdminUser)
                .WithMany(u => u.Hotels)
                .HasForeignKey(f => f.AdminUserId);
        }
    }
}
