using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.SemesterCourseDTOs
{
    public class SemesterCourseDto
    {
        [Required]
        public Guid SemesterId { get; set; }
        [Required]
        public List<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}
