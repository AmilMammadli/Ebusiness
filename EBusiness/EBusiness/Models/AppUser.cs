using Microsoft.AspNetCore.Identity;

namespace EBusiness.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
