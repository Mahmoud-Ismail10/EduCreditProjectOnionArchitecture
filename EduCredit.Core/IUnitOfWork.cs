using EduCredit.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// Generic Method
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> CompleteAsync();
    }
}
