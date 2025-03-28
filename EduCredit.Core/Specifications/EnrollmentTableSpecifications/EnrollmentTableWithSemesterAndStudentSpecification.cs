using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.EnrollmentTableSpecifications
{
    public class EnrollmentTableWithSemesterAndStudentSpecification:BaseSpecifications<EnrollmentTable>

    {
        public EnrollmentTableWithSemesterAndStudentSpecification(Guid studentId)
            : base()
        {
            Includes.Add(e => e.Student);
            Includes.Add(e => e.Semester);
            Includes.Add(e => e.Enrollments);
        }
        public EnrollmentTableWithSemesterAndStudentSpecification(Guid studentId,Guid semesterID)
            : base(e => e.StudentId == studentId&&semesterID==e.SemesterId)
        {
            Includes.Add(e => e.Student);
            Includes.Add(e => e.Semester);
            Includes.Add(e => e.Enrollments);
        }
      
    }
   
}
