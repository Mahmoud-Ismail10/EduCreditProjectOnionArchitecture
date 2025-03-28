using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.StudentDTOs
{
    public class StudentDetailsDto
    {
        public Guid Id { get; set; }
        public string StudentFullName { get; set; }
        public string DepartmentName { get; set; }
        public string NameOfGuide { get; set; }
        public Level Level { get; set; }
        public float Obtainedhours { get; set; }
        public float AvailableHours { get; set; }
        public float GPA { get; set; }

    }
}
