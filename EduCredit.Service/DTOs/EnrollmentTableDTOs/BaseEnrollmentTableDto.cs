using EduCredit.Core.Enums;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.ScheduleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentTableDTOs
{
    public class BaseEnrollmentTableDto
    {
        public string StudentFullName { get; set; }
        public string DepartmentName { get; set; }
        public string NameOfGuide { get; set; }
        public Level Level { get; set; }
        public float Obtainedhours { get; set; }
        public float AvailableHours { get; set; }
        public float GPA { get; set; }

        public List<ReadScheduleDto> AvailableCourses { get; set; } = new List<ReadScheduleDto>();
    }
}
