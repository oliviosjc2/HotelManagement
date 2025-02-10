using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class HotelAdminMapping : IEntityTypeConfiguration<HotelAdmin>
    {
        public void Configure(EntityTypeBuilder<HotelAdmin> entity)
        {
            entity.ToTable("HotelAdmins");

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.HotelAdmins)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.HotelAdminUser)
                .WithMany(hau => hau.HotelAdmins)
                .HasForeignKey(f => f.HotelAdminUserId);
        }
    }
}
