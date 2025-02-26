using EduCredit.Core;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using EduCredit.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EduCreditContext _dbcontext;
        private readonly Hashtable _repo;

        /// Ask CLR for creating object from DbContext and use it in service layer
        public UnitOfWork(EduCreditContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repo = new Hashtable();
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbcontext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            /// if Key is not available create object from TEntity
            var Key = typeof(TEntity).Name;
            if (!_repo.ContainsKey(Key))
            {
                /// we don't need to inject service of IGeneric Repository because we create an object from it manual
                var repository = new GenericRepository<TEntity>(_dbcontext);
                _repo.Add(Key, repository);
            }
            return _repo[Key] as IGenericRepository<TEntity>;
        }
    }
}
