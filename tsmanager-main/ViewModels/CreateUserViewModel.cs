using System.ComponentModel.DataAnnotations;

namespace TSManager.ViewModels
{
    public class CreateUserViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } = "User";
    }
}
