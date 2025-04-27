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
            (!spec.SemesterId.HasValue || d.SemesterCourses.Any(sc => sc.SemesterId == spec.SemesterId.Value)))
        {
            Includes.Add(d => d.Department);
            Includes.Add(d => d.PreviousCourse);
            Includes.Add(d => d.SemesterCourses);
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
            Includes.Add(c => c.SemesterCourses);
        }   
        public CourseWithDeptAndPrevCourseSpecification(Guid? DepartmentId)
            : base(d => !DepartmentId.HasValue|| d.DepartmentId == DepartmentId)
            
        {
           
        }
    }
}
