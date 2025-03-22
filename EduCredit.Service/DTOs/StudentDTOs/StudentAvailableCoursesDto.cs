using EduCredit.Service.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.StudentDTOs
{
    public class StudentAvailableCoursesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string NameofGuide { get; set; }
        public int Level { get; set; }
        public float Obtainedhours  { get; set; }
        public float AvailableHours { get; set; }
        public float GBA { get; set; }

        public List<BaseCourseDto> AvailableCourse { get; set; }

    }
}
