using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Specifications;
using EduCredit.Repository.Data;
using EduCredit.Repository.Specifications.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EduCreditContext _dbcontext;
        
        public GenericRepository(EduCreditContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task CreateAsync(T entity)
        {
            await _dbcontext.Set<T>().AddAsync(entity);
        }

        public Task Delete(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
        public Task Update(T entity)
        {
            _dbcontext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
        public IReadOnlyList<T?> GetAllSpecification(ISpecification<T> spec, out int count)
        {
            return ApplyQuery(spec, out count).AsNoTracking().ToList();
        }

      
        public async Task<T?> GetByIdSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplyQuery(spec, out _).FirstOrDefaultAsync(); /// 'Find' local looking first then looking at DB
        }

        private IQueryable<T> ApplyQuery(ISpecification<T> spec, out int count)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec, out count);
        }
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T?>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbcontext.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T?>> GetAllAsync()
        {
            return await _dbcontext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task CreateRangeAsync(IReadOnlyList<T> entities)
        {
            await _dbcontext.Set<T>().AddRangeAsync(entities);
        }

        public Task DeleteRange(IReadOnlyList<T> entities)
        {
            _dbcontext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync(ISpecification<T>? spec = null)
        {
            IQueryable<T> query = _dbcontext.Set<T>();

            if (spec != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(query, spec,out int count);
            }

            return await query.CountAsync();
        }

    }
}
