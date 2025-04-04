using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Specifications;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class SemeterCourseRepo : GenericRepository<SemesterCourse>, ISemeterCourseRepo
    {
        private readonly EduCreditContext _dbcontext;

        public SemeterCourseRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IReadOnlyList<Guid>> GetSemesterCoursesBySemesterIdAndDepartmentIdAsync(Guid semesterId, Guid? departmentId)
        {
            return await _dbcontext.SemesterCourses
             .Where(sc => sc.SemesterId == semesterId&&sc.Course.DepartmentId == departmentId)
             .Select(s => s.CourseId).ToListAsync();

        }

        //public async Task<IReadOnlyList<Guid>> GetSemesterCoursesBySemesterIdAsync(Guid semesterId)
        //{
        //    return await _dbcontext.SemesterCourses
        //      .Where(sc => sc.SemesterId == semesterId)
        //      .Select(s=>s.CourseId).ToListAsync();
                
        //}
    }
}
