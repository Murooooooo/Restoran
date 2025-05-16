using System.ComponentModel.DataAnnotations;

namespace Restoran.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
      
    }
}
