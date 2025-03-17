using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.DepartmentSpecifications
{
    public class DepartmentWithTeacherSpecifications : BaseSpecifications<Department>
    {
        /// for get all departments (without Criteria)
        public DepartmentWithTeacherSpecifications(DepartmentSpecificationParams spec) : base(d =>
            /// if departmentHeadId HasValue then take value of departmentHeadId else return false(null)
            (string.IsNullOrEmpty(spec.Search) || d.Name.ToLower().Contains(spec.Search.ToLower())) &&
            (!spec.DepartmentHeadId.HasValue || d.DepartmentHeadId == spec.DepartmentHeadId.Value))
        {
            Includes.Add(d => d.DepartmentHead);
            Includes.Add(c => c.Courses);
            if (!string.IsNullOrEmpty(spec.Sort))
            {
                /// if client return another value other than 'nameAsc', 'nameDesc' is the sorting value is 'nameAsc'
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
            /// total departments = 136 ~ 140
            /// page size => 140 / 7(value of Take and page size) = 20 pages
            /// page index => (3(value of page index) - 1) * 7 = 14(value of Skip)
            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
        }
        /// for get one department (with Criteria)
        public DepartmentWithTeacherSpecifications(Guid id) : base(d => d.Id == id)
        {
            Includes.Add(d => d.DepartmentHead);

            Includes.Add(c => c.Courses);
        }
    }
}
