using System.ComponentModel.DataAnnotations;
namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
