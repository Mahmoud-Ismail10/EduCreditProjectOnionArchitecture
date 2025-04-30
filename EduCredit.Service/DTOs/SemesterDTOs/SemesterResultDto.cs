using EduCredit.Service.DTOs.EnrollmentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.SemesterDTOs
{
    public class SemesterResultDto
    {
        public string SemesterName { get; set; }
        public float CreditHours { get; set; }
        public float ObtainedHours { get; set; }
        public double GPA { get; set; }
        public double? Percentage { get; set; }
        public List<ReadEnrollmentDto> Courses { get; set; }
    }
}
