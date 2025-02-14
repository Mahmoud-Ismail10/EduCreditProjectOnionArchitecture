using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class Department
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        /// One-to-one: A department has one head (Teacher)
        public Guid? DepartmentHeadId { get; set; } // Foreign Key allow null
        public Teacher DepartmentHead { get; set; }

        /// One-to-many: Between Department and Student
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();

        /// One-to-many: Between Department and Course
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

        /// One-to-many: Between Department and Teacher
        public ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
    }
}
