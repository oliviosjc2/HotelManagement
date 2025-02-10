using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class SuiteCategoryMapping : IEntityTypeConfiguration<SuiteCategory>
    {
        public void Configure(EntityTypeBuilder<SuiteCategory> entity)
        {
            entity.ToTable("SuiteCategories");

            entity.Property(p => p.Name).HasMaxLength(64).IsRequired();

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.SuiteCategories)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.HotelUser)
                .WithMany(hu => hu.SuiteCategories)
                .HasForeignKey(f => f.HotelUserId);
        }
    }
}
