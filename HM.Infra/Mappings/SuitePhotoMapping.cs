using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class SuitePhotoMapping : IEntityTypeConfiguration<SuitePhoto>
    {
        public void Configure(EntityTypeBuilder<SuitePhoto> entity)
        {
            entity.ToTable("SuitePhotos");

            entity.Property(p => p.BucketURL).HasMaxLength(512).IsRequired();
            entity.Property(p => p.ItsMainPhoto).HasDefaultValue(false).IsRequired();

            entity.HasOne(e => e.Suite)
                .WithMany(s => s.SuitePhotos)
                .HasForeignKey(f => f.SuiteId);

            entity.HasOne(e => e.HotelUser)
                .WithMany(hu => hu.SuitePhotos)
                .HasForeignKey(f => f.HotelUserId);

        }
    }
}
