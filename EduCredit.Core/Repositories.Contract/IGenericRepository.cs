using EduCredit.Core.Models;
using EduCredit.Core.Specifications;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdSpecificationAsync(ISpecification<T> spec);
        Task<T?> GetByIdAsync(Guid id);
        IReadOnlyList<T?> GetAllSpecification(ISpecification<T> spec, out int count);
        Task<IReadOnlyList<T?>> GetAllAsync();
        Task CreateAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task CreateRangeAsync(IReadOnlyList<T> entities);
        Task DeleteRange(IReadOnlyList<T> entities);
        Task<int> CountAsync(ISpecification<T>? spec = null);
    }
}
