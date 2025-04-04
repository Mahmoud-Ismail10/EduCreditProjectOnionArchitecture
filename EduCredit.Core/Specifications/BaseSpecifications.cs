using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; set; } // Can equal null
        
        public List<Expression<Func<T, object>>> Includes { get; set; } // Can't equal null, and must initialize it in constructor
            = new List<Expression<Func<T, object>>>();
        public List<string> ThenIncludes { get; set; } // Can't equal null, and must initialize it in constructor
            = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; set; } // null
        public Expression<Func<T, object>> OrderByDesc { get; set; } // null
        public Expression<Func<T, object>> ThenBy { get; set; } // null
        public Expression<Func<T, object>> ThenByDesc { get; set; } // null
        public int Skip { get; set; } // 0
        public int Take { get; set; } // 0
        public bool IsPaginationEnabled { get; set; } = false;

        public BaseSpecifications()
        {
            /// Critria = null
        }
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression) // Just setter for OrderBy property
        {
            OrderBy = orderByExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression) // Just setter for OrderByDesc property
        {
            OrderByDesc = orderByDescExpression;
        }
        public void AddThenBy(Expression<Func<T, object>> thenByExpression)
        {
            ThenBy = thenByExpression;
        }
        public void AddThenByDesc(Expression<Func<T, object>> thenByDescExpression)
        {
            ThenByDesc = thenByDescExpression;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
