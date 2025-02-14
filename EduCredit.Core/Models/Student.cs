using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class Student : Person
    {
        public float CreditHours { get; set; }
        public float GPA { get; set; }
        public byte Level { get; set; }

        /// One-to-many: Between Student and Department
        public Guid DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }

        /// One-to-many: Between Student and Teacher (Guidance)
        public Guid TeacherId { get; set; } // Foreign Key
        public Teacher Teacher { get; set; }

        /// Many-to-many: Between Student and Course (JoinTable)
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
    }
}
