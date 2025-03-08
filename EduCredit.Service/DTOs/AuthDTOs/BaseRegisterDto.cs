using EduCredit.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class BaseRegisterDto
    {
      
        [Required(ErrorMessage = "Full Name is required")]
        [MinLength(3, ErrorMessage = "Full Name must be at least 3 characters")]
        [MaxLength(60, ErrorMessage = "Full Name cannot exceed 60 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full Name must contain only letters and spaces")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MinLength(5, ErrorMessage = "Address must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        [RegularExpression(@"^\d{10,14}$", ErrorMessage = "National ID must be between 10 and 14 digits")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        public DateOnly BirthDate { get; set; }
    }


}

