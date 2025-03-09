using EduCredit.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Roles Role { get; set; }
    }
}
