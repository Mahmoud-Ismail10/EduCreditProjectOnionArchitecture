using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Service.Helper;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.EnrollmentDTOs
{
    public class BaseEnrollmentDto
    {
        [Required]
        public Guid EnrollmentTableId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
    }
}
