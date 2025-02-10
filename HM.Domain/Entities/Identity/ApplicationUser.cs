using Microsoft.AspNetCore.Identity;

namespace HM.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresIn { get; set; }
        public virtual ICollection<Hotel>? Hotels { get; set; }
        public virtual ICollection<HotelPhoto>? HotelPhotos { get; set; }
        public virtual ICollection<HotelEmployees>? HotelEmployees { get; set; }  
        public virtual ICollection<HotelAdmin>? HotelAdmins { get; set; }  
        public virtual ICollection<SuiteCategory>? SuiteCategories { get; set; }
        public virtual ICollection<Suite>? Suites { get; set; }
        public virtual ICollection<SuitePhoto>? SuitePhotos { get; set; }   
        public virtual ICollection<Reserve>? Reserves { get; set; }
    }
}
