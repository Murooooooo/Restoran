using System.ComponentModel.DataAnnotations;

namespace Restoran.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "UserName or Email is required")]
        [StringLength(50, ErrorMessage = "UserName or Email cannot be longer than 50 characters.")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
