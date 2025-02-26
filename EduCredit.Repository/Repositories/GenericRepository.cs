using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Specifications;
using EduCredit.Repository.Data;
using EduCredit.Repository.Specifications.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Delete(Guid id)
        {
            var entity = _dbcontext.Set<T>().Find(id);
            if (entity != null)
            {
                _dbcontext.Set<T>().Remove(entity);
            }
        }

        public void Update(T entity)
        {
            _dbcontext.Set<T>().Update(entity);
        }

        public IReadOnlyList<T?> GetAllSpecification(ISpecification<T> spec, out int count)
        {
            return ApplyQuery(spec, out count).ToList();
        }

        public async Task<T?> GetByIdSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplyQuery(spec, out _).FirstOrDefaultAsync(); /// 'Find' local looking first then looking at DB
        }

        private IQueryable<T> ApplyQuery(ISpecification<T> spec, out int count)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec, out count);
        }

        
    }
}
