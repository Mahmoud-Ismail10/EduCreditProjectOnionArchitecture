using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public float CreditHours { get; set; }
        public float MinimumDegree { get; set; }
        public int Duration { get; set; }

        /// Self-reference to represent the previous course
        public Guid? PreviousCourseId { get; set; } // Foreign Key allow null
        public Course PreviousCourse { get; set; }

        /// One-to-many: Between Course and Department
        public Guid DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }

        /// One-to-One: Between Course and Schedule
        public Schedule Schedule { get; set; }

        /// Many-to-many: Between Course and EnrollmentTable (JoinTable)
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();

        /// Many-to-many: Between Course and Semester (JoinTable)
        public ICollection<SemesterCourse> SemesterCourses { get; set; } = new HashSet<SemesterCourse>();
    }
}
