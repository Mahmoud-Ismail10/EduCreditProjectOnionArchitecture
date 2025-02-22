using EduCredit.Core.Enums;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class EnrollmentTable
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public DateOnly Session { get; set; }
        public Semester Semester { get; set; }

        /// One-to-many: Between EnrollmentTable and Student
        public Guid StudentId { get; set; } // Foreign Key
        public Student Student { get; set; }

        /// Many-to-many: Between EnrollmentTable and Course (JoinTable)
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
    }
}
