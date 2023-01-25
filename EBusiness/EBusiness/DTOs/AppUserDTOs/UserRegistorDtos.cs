using System.ComponentModel.DataAnnotations;

namespace EBusiness.DTOs.AppUserDTOs
{
    public class UserRegistorDtos
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
