using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.EnrollmentsSpecifications
{
    public class EnrollmentsWithCoursesSpecification : BaseSpecifications<Enrollment>
    {
        private Guid? enrollmentTableId;

        public EnrollmentsWithCoursesSpecification(Guid? enrollmentTableId, Guid? semesterId)
     : base(e =>
         (!enrollmentTableId.HasValue || e.EnrollmentTableId == enrollmentTableId.Value) &&
         (!semesterId.HasValue || e.EnrollmentTable.SemesterId == semesterId.Value))
        {
            Includes.Add(e => e.Course);
        }

        public EnrollmentsWithCoursesSpecification(Guid? departmentId, bool passedOnly = false)
    : base(e =>
        (!departmentId.HasValue || e.Course.DepartmentId == departmentId.Value) &&
        (!passedOnly || e.IsPassAtCourse == true)
    )
        {
        }
        public EnrollmentsWithCoursesSpecification(EnrollmentSpecificationParams spec) :
            base
             (e=>(!spec.semesterId.HasValue||e.EnrollmentTable.SemesterId==spec.semesterId))
        {
            Includes.Add(d => d.Course);
            ThenIncludes.Add("EnrollmentTable.Student");
            if (!string.IsNullOrEmpty(spec.Sort))
            {
                /// if client return another value other than 'nameAsc', 'nameDesc' is the sorting value is 'nameAsc'
                switch (spec.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(d => d.EnrollmentTable.Semester.Name); break;
                    case "namedesc":
                        AddOrderByDesc(d => d.EnrollmentTable.Semester.Name); break;
                    default:
                        AddOrderBy(d => d.EnrollmentTable.Semester.EndDate); break;
                }
                AddOrderBy(d => d.EnrollmentTable.Semester.EndDate);
            }
        }
    }   

}
