using EduCredit.Core.Models;
namespace EduCredit.Core.Specifications.AdminSpecifications
{
    public class AdminSpecification : BaseSpecifications<Admin>
    {
        /// for get all admins (without Criteria)
        public AdminSpecification(AdminSpecificationParams spec) : base(d =>
            (string.IsNullOrEmpty(spec.Search) || d.FullName.ToLower().Contains(spec.Search.ToLower())))
        {
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
        /// for get one admin (with Criteria)
        public AdminSpecification(Guid id) : base(d => d.Id == id)
        {
           
        }
    }
}
