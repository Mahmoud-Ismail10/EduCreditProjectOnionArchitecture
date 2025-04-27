using EduCredit.Core.Models;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.TeacherSpecefications
{
    public class TeacherWithDepartmentSpecifications : BaseSpecifications<Teacher>
    {
        /// for get all teachers (without Criteria)
        public TeacherWithDepartmentSpecifications(TeacherSpecificationParams spec) : base(d =>
            (string.IsNullOrEmpty(spec.Search) || d.FullName.ToLower().Contains(spec.Search.ToLower())) &&
            (!spec.DepartmentId.HasValue || d.DepartmentId == spec.DepartmentId.Value))
        {
            Includes.Add(d => d.Department);
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
        /// for get one teacher (with Criteria)
        public TeacherWithDepartmentSpecifications(Guid id) : base(d => d.Id == id)
        {
            Includes.Add(d => d.Department);
        } 
        public TeacherWithDepartmentSpecifications(Guid? DepartmrntId) 
            : base(d => d.DepartmentId == DepartmrntId)
        {
        }
    }
}
