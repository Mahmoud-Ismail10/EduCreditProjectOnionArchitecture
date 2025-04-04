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
    public class StudentRepo : GenericRepository<Student>, IStudentRepo
    {
        private readonly EduCreditContext _dbcontext;

        public StudentRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //public async Task<IReadOnlyList<Student>> GetStudentsByTeacherIdAsync(Guid teacherId)
        //{
        //    var students = await _dbcontext.Students.Where(x => x.TeacherId == teacherId).ToListAsync();
        //    return students;
        //}
    }
}
