using EduCredit.Core.Models;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.CourseSpecifications
{
    public class CourseWithDeptAndPrevCourseSpecification : BaseSpecifications<Course>
    {
        /// for get all courses (without Criteria)
        public CourseWithDeptAndPrevCourseSpecification(CourseSpecificationParams spec) : base(d =>
            (string.IsNullOrEmpty(spec.Search) || d.Name.ToLower().Contains(spec.Search.ToLower())) &&
            (!spec.DepartmentId.HasValue || d.DepartmentId == spec.DepartmentId.Value) &&
            (!spec.PreviousCourseId.HasValue || d.PreviousCourseId == spec.PreviousCourseId.Value)&&
            (!spec.SemesterId.HasValue || d.Schedules.Any(sc => sc.SemesterId == spec.SemesterId.Value))&&
            (!spec.StudentId.HasValue || d.Enrollments.Any(e => e.EnrollmentTable.StudentId == spec.StudentId.Value))
        )
        {
            Includes.Add(d => d.Department);
            Includes.Add(d => d.PreviousCourse);
            Includes.Add(d => d.Schedules);
            ThenIncludes.Add("Department.Students");
            if (!string.IsNullOrEmpty(spec.Sort))
            {
                switch (spec.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(d => d.Name); break;
                    case "namedesc":
                        AddOrderByDesc(d => d.Name); break;
                    default:
                        AddOrderBy(d => d.Name); break;
                }
            }
            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
        }
        /// for get all course in the same semester (with Criteria)
        public CourseWithDeptAndPrevCourseSpecification(Guid id)
            : base(d => d.Id == id)
        {
            Includes.Add(d => d.Department);
            Includes.Add(d => d.PreviousCourse);
            Includes.Add(c => c.Schedules);
        }
        public CourseWithDeptAndPrevCourseSpecification(Guid? DepartmentId, Guid? teacherId = null,Guid? CurrentSemesterId=null)
            : base(d => (!DepartmentId.HasValue || d.DepartmentId == DepartmentId) &&
                  (!teacherId.HasValue || d.Schedules.SelectMany(t => t.TeacherSchedules).Select(s => s.TeacherId).Any(t => t == teacherId)
            )
            && (!CurrentSemesterId.HasValue || d.Schedules.Any(s => s.SemesterId == CurrentSemesterId))
            )

        {
           
        }
    }
}
