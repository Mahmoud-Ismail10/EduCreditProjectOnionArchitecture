using EduCredit.Core;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using EduCredit.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly EduCreditContext _db;
        private readonly Hashtable _repo;

        public UnitOfWork(EduCreditContext db)
        {
            _db = db;
            _repo = new Hashtable();
        }

        public async Task<int> completeAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _db.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            //if Key is not available create object from TEntity
            var Key = typeof(TEntity).Name;
            if (!_repo.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity>(_db);
                _repo.Add(Key, repository);
            }
            return _repo[Key] as IGenericRepository<TEntity>;
        }
    }
}
