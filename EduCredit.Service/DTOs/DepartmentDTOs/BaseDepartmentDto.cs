using EduCredit.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.DepartmentDTOs
{
    public class BaseDepartmentDto
    {
        [Required]
        [RegularExpression(pattern: "^[A-Za-z\\s]{3,}$",
                           ErrorMessage = "Name must be char only and more than 2 char")]
        public string Name { get; set; }
        public Guid? DepartmentHeadId { get; set; }
    }
}
