using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Service.Helper;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.EnrollmentDTOs
{
    public class BaseEnrollmentDto
    {
        public float Grade { get; set; }
        public float? Percentage { get; set; }
        public Appreciation? Appreciation { get; set; }
        public bool? IsPassAtCourse { get; set; }
    }
}
