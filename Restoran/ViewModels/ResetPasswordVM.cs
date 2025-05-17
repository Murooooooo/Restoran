using System.ComponentModel.DataAnnotations;

namespace Restoran.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
        public string? userId { get; set; }
        public string? token { get; set; }
    }
}
