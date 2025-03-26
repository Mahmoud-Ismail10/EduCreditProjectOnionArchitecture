using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Relations
{
    public class Enrollment
    {
        public Guid EnrollmentTableId { get; set; }
        public EnrollmentTable EnrollmentTable { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public float? Grade { get; set; }
        public float? Percentage { get; set; }
        public Appreciation? Appreciation { get; set; }
        public bool? IsPassAtCourse { get; set; }
    }
}
