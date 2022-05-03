using System.ComponentModel.DataAnnotations;

namespace ThingiClone.Models.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
