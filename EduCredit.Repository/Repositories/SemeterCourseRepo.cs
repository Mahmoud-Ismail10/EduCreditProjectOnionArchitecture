using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Specifications;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public async Task<IReadOnlyList<Course>> GetCoursesByTeacherIdAsync(Guid teacherId,Guid semesterID)
        {
            var courses = _dbcontext.SemesterCourses
                .Where(sc => sc.SemesterId == semesterID)
                .Where(t=>t.Course.Schedules.SelectMany(s=>s.TeacherSchedules).Where(s=>s.TeacherId==teacherId).Any())
                .Include(E=>E.Course.Enrollments)
                .Select(s=>s.Course)
                .ToListAsync();
            if (courses is null) return null;
            return await courses;
        }

        public async Task<IReadOnlyList<Guid>> GetSemesterCoursesBySemesterIdAndDepartmentIdAsync(Guid semesterId, Guid? departmentId)
        {
            return await _dbcontext.SemesterCourses
             .Where(sc => sc.SemesterId == semesterId&&sc.Course.DepartmentId == departmentId)
             .Select(s => s.CourseId).ToListAsync();

        }
    }
}
