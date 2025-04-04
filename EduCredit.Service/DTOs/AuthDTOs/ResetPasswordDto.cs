using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [PasswordPropertyText]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
