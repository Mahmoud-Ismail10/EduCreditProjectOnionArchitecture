using EduCredit.Service.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.DepartmentDTOs
{
    public class ReadDepartmentCoursesDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public List<departmentcourseDto> Courses { get; set; }= new List<departmentcourseDto>();
    }
}
