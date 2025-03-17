using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.StudentDTOs
{
    public class ReadStudentDto : BaseRegisterDto
    {
        public Guid Id { get; set; }
        public float CreditHours { get; set; }
        public float GPA { get; set; }
        public Level Level { get; set; }
        public string DepartmentName { get; set; }
        public string AcademicGuide { get; set; }
    }
}
