using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications
{
    public interface ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; set; } /// x => x.id == id (Where)
        public List<Expression<Func<T, object>>> Includes { get; set; } /// {x => x.Courses, x => x.Students} (Include)
        public List<string> ThenIncludes { get; set; } /// {x => x.Courses, x => x.Students} (Include)
        public Expression<Func<T, object>> OrderBy { get; set; } /// x => x.Name (OrderByAsc) 
        public Expression<Func<T, object>> OrderByDesc { get; set; } /// x => x.Name (OrderByDesc) 
        public Expression<Func<T, object>> ThenBy { get; set; } /// x => x.Name (ThenByAsc)
        public Expression<Func<T, object>> ThenByDesc { get; set; } /// x => x.Name(ThenByDesc)
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
