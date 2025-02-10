using HM.Domain.Entities;
using HM.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HM.Infra.Context
{
    public class HMContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public HMContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public HMContext(DbContextOptions<HMContext> options)
        : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("hm");

            Assembly assemblyWithConfigurations = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(21);
                property.SetScale(2);
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
                {
                    property.SetColumnType("timestamp");
                }
            }

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        }

        public virtual DbSet<Hotel>? Hotels { get; set; }
        public virtual DbSet<HotelEmployees>? HotelEmployees { get; set; }
        public virtual DbSet<HotelAdmin>? HotelAdmins { get; set; }
        public virtual DbSet<HotelPhoto>? HotelPhotos { get; set; }
        public virtual DbSet<Invoice>? Invoices { get; set; }
        public virtual DbSet<Reserve>? Reserves { get; set; }
        public virtual DbSet<ReserveCustomerInformations>? ReserveCustomerInformations { get; set; }
        public virtual DbSet<Suite>? Suites { get; set; }
        public virtual DbSet<SuiteCategory>? SuiteCategories { get; set; }
        public virtual DbSet<SuitePhoto>? SuitePhotos { get; set; }
        public virtual DbSet<SuiteSchedule>? SuiteSchedules { get; set; }
    }
}