using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class Teacher : Person
    {
        public DateOnly AppointmentDate { get; set; }

        /// One-to-many: Between Teacher and Department
        public Guid? DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }

        /// One-to-many: Between Teacher and Student (Guidance)
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();

        /// Optional navigation property for one-to-one relationship (if the teacher became head)
        public Department? HeadofDepartment { get; set; }

        /// Many-to-many: Between Teacher and Schedule (JoinTable)
        public ICollection<TeacherSchedule> TeacherSchedules { get; set; } = new HashSet<TeacherSchedule>();
    }
}
