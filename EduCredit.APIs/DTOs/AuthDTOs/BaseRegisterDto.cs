using EduCredit.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.APIs.DTOs.AuthDTOs
{
    public class BaseRegisterDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string NationalId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }
    }
}
