using System.ComponentModel.DataAnnotations;
namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }
    }
}
