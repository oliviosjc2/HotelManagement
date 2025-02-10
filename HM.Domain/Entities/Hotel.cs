using HM.Domain.Entities.Base;
using HM.Domain.Entities.Identity;

namespace HM.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        public Hotel()
        {
            
        }

        public Hotel(string name, 
            string email, 
            bool acceptMinors, 
            int bookingConfirmationTimeInMinutes,
            ApplicationUser adminUser)
        {
            Name = name;
            Email = email;
            AcceptMinors = acceptMinors;
            BookingConfirmationTimeInMinutes = bookingConfirmationTimeInMinutes;
            AdminUser = adminUser;
            Actived = true;
            CreatedAt = DateTime.UtcNow;
        }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool AcceptMinors { get; set; } = default;
        public int BookingConfirmationTimeInMinutes { get; set; } = default;
        public int AdminUserId { get; set; } = default;
        public virtual ICollection<SuiteCategory>? SuiteCategories { get; set; }
        public virtual ICollection<Suite>? Suites { get; set; }
        public virtual ICollection<HotelPhoto>? HotelPhotos { get; set; }
        public virtual ApplicationUser? AdminUser { get; set; }
        public virtual ICollection<HotelEmployees>? HotelEmployees { get; set; }
        public virtual ICollection<HotelAdmin>? HotelAdmins { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<Reserve>? Reserves { get; set; }
    }
}
