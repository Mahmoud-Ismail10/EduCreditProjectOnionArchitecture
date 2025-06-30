using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class Semester
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime EnrollmentOpen { get; set; }
        public DateTime EnrollmentClose { get; set; }
        public bool IsCourseGroupsCleared { get; set; } = false;
        public bool IsEnrollmentTablesCleared { get; set; } = false;


        /// One-to-many: Between Semester and EnrollmentTanle
        public ICollection<EnrollmentTable> EnrollmentTables { get; set; } = new HashSet<EnrollmentTable>();
        
        /// Many-to-many: Between Semester and Course (JoinTable)
        public ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
    }
}
