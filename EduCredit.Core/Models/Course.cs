using EduCredit.Core.Chat;
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

        /// One-to-Many: Self-reference to represent the previous course
        public Guid? PreviousCourseId { get; set; } // Foreign Key allow null
        public Course PreviousCourse { get; set; }
        public ICollection<Course> NextCourses { get; set; } = new HashSet<Course>();

        /// One-to-many: Between Course and Department
        public Guid DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }

        /// Many-to-many: Between Course and EnrollmentTable (JoinTable)
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();

        /// Many-to-many: Between Course and Semester (JoinTable)
        public ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();

        /// One-to-many: Between Course and ChatMessage
        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
    }
}
