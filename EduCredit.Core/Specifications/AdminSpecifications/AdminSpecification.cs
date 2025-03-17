using EduCredit.Core.Models;
namespace EduCredit.Core.Specifications.AdminSpecifications
{
    public class AdminSpecification : BaseSpecifications<Admin>
    {
        /// for get all departments (without Criteria)
        public AdminSpecification(AdminSpecificationParams spec) : base(d =>
            /// if departmentHeadId HasValue then take value of departmentHeadId else return false(null)
            (string.IsNullOrEmpty(spec.Search) || d.FullName.ToLower().Contains(spec.Search.ToLower())))
        {
            if (!string.IsNullOrEmpty(spec.Sort))
            {
                /// if client return another value other than 'nameAsc', 'nameDesc' is the sorting value is 'nameAsc'
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
            /// total departments = 136 ~ 140
            /// page size => 140 / 7(value of Take and page size) = 20 pages
            /// page index => (3(value of page index) - 1) * 7 = 14(value of Skip)
            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
        }
        /// for get one department (with Criteria)
        public AdminSpecification(Guid id) : base(d => d.Id == id)
        {
           
        }
    }
}
