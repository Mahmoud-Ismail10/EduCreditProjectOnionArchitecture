using EduCredit.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EduCredit.Service.DTOs.DepartmentDTOs
{
    public class BaseDepartmentDto
    {
        [Required(ErrorMessage = "Department Name is required")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [RegularExpression(pattern: "^[A-Za-z\\s]{3,}$", ErrorMessage = "Name must contain only letters and spaces")]
        public string Name { get; set; }
    }
}
