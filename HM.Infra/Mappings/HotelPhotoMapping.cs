using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class HotelPhotoMapping : IEntityTypeConfiguration<HotelPhoto>
    {
        public void Configure(EntityTypeBuilder<HotelPhoto> entity)
        {
            entity.ToTable("HotelPhotos");

            entity.Property(p => p.ItsMainPhoto).HasDefaultValue(false).IsRequired();
            entity.Property(p => p.BucketURL).HasMaxLength(512).IsRequired();

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.HotelPhotos)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.HotelUser)
                .WithMany(hu => hu.HotelPhotos)
                .HasForeignKey(f => f.HotelUserId);
        }
    }
}
