using EduCredit.Service.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.CourseDTOs
{
    public class departmentcourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<StudentGradeDto> Students { get; set; } = new List<StudentGradeDto>();
    }
}
