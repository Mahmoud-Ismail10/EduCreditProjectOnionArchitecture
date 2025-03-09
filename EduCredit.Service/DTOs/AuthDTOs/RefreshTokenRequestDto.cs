using System.ComponentModel.DataAnnotations;
namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
