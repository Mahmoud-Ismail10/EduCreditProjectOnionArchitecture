using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class TeacherRepo : GenericRepository<Teacher>, ITeacherRepo
    {
        private readonly EduCreditContext _dbcontext;

        public TeacherRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IReadOnlyList<Teacher?> GetTeachersAreNotReachMaximumOfStudentsByDepartmentId(Guid? departmentId)
        {
            var teachers = _dbcontext.Teachers
                .Where(t => t.DepartmentId == departmentId)
                .Where(t => t.Students.Count(s => s.Level == Level.First) < 4)
                .AsNoTracking() // Improves performance if you don’t need to update entities
                .ToList();

            return teachers;
        }
    }
}
