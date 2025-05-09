using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.StudentDTOs
{
    public class StudentGradeDto
    {
        public Guid EnrollmentTableId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }

        public float? grade { get; set; }

    }
}