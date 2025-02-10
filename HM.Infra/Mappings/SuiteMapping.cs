using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class SuiteMapping : IEntityTypeConfiguration<Suite>
    {
        public void Configure(EntityTypeBuilder<Suite> entity)
        {
            entity.ToTable("Suites");

            entity.Property(p => p.Name).IsRequired().HasMaxLength(64);
            entity.Property(p => p.Description).HasMaxLength(255);
            entity.Property(p => p.PeopleCapacity).HasDefaultValue(10).IsRequired();
            entity.Property(p => p.DailyPriceDefault).HasDefaultValue(10).IsRequired();

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.Suites)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.SuiteCategory)
                .WithMany(sc => sc.Suites)
                .HasForeignKey(f => f.SuiteCategoryId);

            entity.HasOne(e => e.HotelUser)
                .WithMany(hu => hu.Suites)
                .HasForeignKey(f => f.HotelUserId);
        }
    }
}
