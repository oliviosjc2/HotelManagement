using HM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HM.Infra.Mappings
{
    public class HotelEmployeeMapping : IEntityTypeConfiguration<HotelEmployees>
    {
        public void Configure(EntityTypeBuilder<HotelEmployees> entity)
        {
            entity.ToTable("HotelEmployees");

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.HotelEmployees)
                .HasForeignKey(f => f.HotelId);

            entity.HasOne(e => e.EmployeeUser)
                .WithMany(eu => eu.HotelEmployees)
                .HasForeignKey(f => f.EmployeeUserId);
        }
    }
}
