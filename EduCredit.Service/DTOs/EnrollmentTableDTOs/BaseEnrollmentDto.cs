using EduCredit.Core.Enums;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentTableDTOs
{
    public class BaseEnrollmentDto
    {
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string NameOfGuide { get; set; }
        public Level Level { get; set; }
        public float Obtainedhours { get; set; }
        public float AvailableHours { get; set; }
        public float GBA { get; set; }

        public List<ReadCourseDto> semesterCourses { get; set; }=new List<ReadCourseDto>();
        //public List<Guid> semesterCourses { get; set; }=new List<Guid>();




    }
}
