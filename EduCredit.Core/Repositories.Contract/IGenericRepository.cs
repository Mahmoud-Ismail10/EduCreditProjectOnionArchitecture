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
        Task<T> GetByIdSpecificationAsync(ISpecification<T> spec);
        IReadOnlyList<T> GetAllSpecification(ISpecification<T> spec, out int count);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
