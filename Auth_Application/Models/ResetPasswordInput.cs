
using System.ComponentModel.DataAnnotations;
namespace Auth_Application.Models
{
    public class ResetPasswordInput
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        [MaxLength(10, ErrorMessage = "Password Max length 10 charcters")]
        [MinLength(6, ErrorMessage = "Password Min length 6 charcters")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords Not Matche")]
        [MaxLength(10, ErrorMessage = "Password Max length 10 charcters")]
        [MinLength(6, ErrorMessage = "Password Min length 6 charcters")]
        public string ConfirmPassword { get; set; }
    }
}
