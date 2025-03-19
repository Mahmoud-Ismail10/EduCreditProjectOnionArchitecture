using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Service.Helper;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.EnrollmentDTOs
{
    public class EnrollmentDto
    {
        [Required]
        public Guid EnrollmentTableId { get; set; }
        [Required]
        public List<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}
