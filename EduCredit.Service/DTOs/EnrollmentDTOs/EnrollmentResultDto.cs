using EduCredit.Service.DTOs.SemesterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentDTOs
{
    public class EnrollmentResultDto
    {
        public string StudentName { get; set; }
        public double GPA { get; set; }
        public float ObtainedHours { get; set; }
        public float CreditHours { get; set; }
        public float TotalObtainedHours { get; set; }
        public double TotalPercentage { get; set; }
        public List<SemesterResultDto> Semesters { get; set; }
    }

}
