using EduCredit.Core.Models;
using EduCredit.Core.Specifications.TeacherSpecefications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.StudentSpecifications
{
    public class StudentWithDepartmentAndGuideSpecification : BaseSpecifications<Student>
    {
        /// for get all students (without Criteria)
        public StudentWithDepartmentAndGuideSpecification(StudentSpecificationParams spec) : base(d =>
            (string.IsNullOrEmpty(spec.Search) || d.FullName.ToLower().Contains(spec.Search.ToLower())) &&
            (!spec.DepartmentId.HasValue || d.DepartmentId == spec.DepartmentId.Value) &&
            (!spec.AcademicGuideId.HasValue || d.TeacherId == spec.AcademicGuideId.Value))

        {
            Includes.Add(d => d.Department);
            Includes.Add(t => t.Teacher); // Academic Guide
            Includes.Add(t => t.EnrollmentTables);

            if (!string.IsNullOrEmpty(spec.Sort))
            {
                switch (spec.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(d => d.FullName); break;
                    case "namedesc":
                        AddOrderByDesc(d => d.FullName); break;
                    default:
                        AddOrderBy(d => d.FullName); break;
                }
            }
            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
        }
        /// for get one student (with Criteria)
        public StudentWithDepartmentAndGuideSpecification(Guid id) 
            : base(d => d.Id == id)
            
        {
            Includes.Add(d => d.Department);
            Includes.Add(t => t.Teacher); // Academic Guide
        }
    }
}
