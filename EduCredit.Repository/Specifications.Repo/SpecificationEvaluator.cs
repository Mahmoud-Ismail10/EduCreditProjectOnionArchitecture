using EduCredit.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Specifications.Repo
{
    internal static class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> sequence, ISpecification<TEntity> spec, out int count)
        {
            var query = sequence; // _dbcontext.Set<className>()
            /// Where => Fileration
            if (spec.Criteria is not null) // x => x.id == id
            {
                query = query.Where(spec.Criteria); // _dbcontext.Set<className>().Where(x => x.id == id)
            }

            /// Get count of list before apply specifications
            count = query.Count();

            /// OrderBy => Sorting
            if (spec.OrderBy is not null)
            {
                if (spec.ThenBy is not null)
                {
                    query = query.OrderBy(spec.OrderBy).ThenBy(spec.ThenBy); // _dbcontext.Set<className>().Where(x => x.id == id).OrderBy(x => x.name).ThenBy(x => x.anotherAttribute);
                }
                else if (spec.ThenByDesc is not null)
                {
                    query = query.OrderBy(spec.OrderBy).ThenByDescending(spec.ThenByDesc); // _dbcontext.Set<className>().Where(x => x.id == id).OrderBy(x => x.name).ThenByDescending(x => x.anotherAttribute);
                }
                else
                {
                    query = query.OrderBy(spec.OrderBy); // _dbcontext.Set<className>().Where(x => x.id == id).OrderBy(x => x.name);
                }
            }
            else if (spec.OrderByDesc is not null)
            {
                if (spec.ThenBy is not null)
                {
                    query = query.OrderByDescending(spec.OrderByDesc).ThenBy(spec.ThenBy); // _dbcontext.Set<className>().Where(x => x.id == id).OrderByDescending(x => x.name).ThenBy(x => x.anotherAttribute);
                }
                else if (spec.ThenByDesc is not null)
                {
                    query = query.OrderByDescending(spec.OrderByDesc).ThenByDescending(spec.ThenByDesc); // _dbcontext.Set<className>().Where(x => x.id == id).OrderByDescending(x => x.name).ThenByDescending(x => x.anotherAttribute);
                }
                else
                {
                    query = query.OrderByDescending(spec.OrderByDesc); // _dbcontext.Set<className>().Where(x => x.id == id).OrderByDescending(x => x.name);
                }
            }

            /// Skip, Take => Pagination
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (currentQuery, includesExpressions) => currentQuery.Include(includesExpressions));
            // _dbcontext.Set<className>().Where(x => x.id == id).Include(x => x.className).Include(x => x.className)...
            return query;
        }
    }
}
