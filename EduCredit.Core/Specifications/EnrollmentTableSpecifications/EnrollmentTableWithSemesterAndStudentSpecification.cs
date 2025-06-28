using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.EnrollmentTableSpecifications
{
    public class EnrollmentTableWithSemesterAndStudentSpecification : BaseSpecifications<EnrollmentTable>
    {
        //public EnrollmentTableWithSemesterAndStudentSpecification(Guid enrollmentTableId)
        //    : base(e => e.Id == enrollmentTableId)
        //{
        //    Includes.Add(e => e.Student);
        //    Includes.Add(e => e.Semester);
        //    Includes.Add(e => e.Enrollments);
        //}
        //public EnrollmentTableWithSemesterAndStudentSpecification(Guid studentId)
        //    : base()
        //{
        //    Includes.Add(e => e.Student);
        //    Includes.Add(e => e.Semester);
        //    Includes.Add(e => e.Enrollments);
        //}
        public EnrollmentTableWithSemesterAndStudentSpecification(Guid studentId, Guid semesterID)
            : base(e => e.StudentId == studentId && semesterID == e.SemesterId)
        {
            Includes.Add(e => e.Student);
            Includes.Add(e => e.Semester);
            Includes.Add(e => e.Enrollments);
            ThenIncludes.Add("Student.Department");
        }

        /// Static factory methods to create specifications based on different criteria
        public EnrollmentTableWithSemesterAndStudentSpecification(Expression<Func<EnrollmentTable, bool>> criteria)
          : base(criteria)
        {
            Includes.Add(e => e.Student);
            Includes.Add(e => e.Semester);
            Includes.Add(e => e.Enrollments);
        }

        public static EnrollmentTableWithSemesterAndStudentSpecification ByEnrollmentTableId(Guid enrollmentTableId)
        {
            return new EnrollmentTableWithSemesterAndStudentSpecification(e => e.Id == enrollmentTableId);
        }

        public static EnrollmentTableWithSemesterAndStudentSpecification ByStudentId(Guid studentId)
        {
            return new EnrollmentTableWithSemesterAndStudentSpecification(e => e.StudentId == studentId);
        }
    }
   
}
