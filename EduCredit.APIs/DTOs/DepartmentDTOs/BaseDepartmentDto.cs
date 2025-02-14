using EduCredit.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.APIs.DTOs.DepartmentDTOs
{
    public class BaseDepartmentDto
    {
        [Required]
        [MaxLength(20)]
        [Display(Name = "Department Name")]
        [RegularExpression(pattern: "^[A-Za-z ]+$",
                           ErrorMessage = "Name must be char only and more than 2 char")]
        public string Name { get; set; }

        [Display(Name = "Department Head")]
        public Guid? DepartmentHeadId { get; set; }
        public string DepartmentHeadName { get; set; }
    }
}
