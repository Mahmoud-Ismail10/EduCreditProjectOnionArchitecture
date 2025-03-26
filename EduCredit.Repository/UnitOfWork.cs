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
        public IEnrollmentTableRepo _enrollmentTableRepo { get; }
        public IEnrollmentRepo _enrollmentRepo { get; }
        public IScheduleRepo _scheduleRepo { get; }
        public ISemesterRepo _semesterRepo { get; }
        public ICourseRepo _courseRepo { get; }
        public ITeacherRepo _teacherRepo { get; }

        /// Ask CLR for creating object from DbContext and use it in service layer
        public UnitOfWork(EduCreditContext dbcontext,
            IEnrollmentTableRepo enrollmentTableRepo,
            IEnrollmentRepo enrollmentRepo,
            IScheduleRepo scheduleRepo,
            ISemesterRepo semesterRepo,
            ITeacherRepo teacherRepo,
            ICourseRepo courseRepo)
        {
            _dbcontext = dbcontext;
            _repo = new Hashtable();
            _enrollmentTableRepo = enrollmentTableRepo;
            _enrollmentRepo = enrollmentRepo;
            _scheduleRepo = scheduleRepo;
            _semesterRepo = semesterRepo;
            _teacherRepo = teacherRepo;
            _courseRepo = courseRepo;
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
