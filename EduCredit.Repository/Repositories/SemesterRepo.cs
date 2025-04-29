using EduCredit.Core.Models;
using EduCredit.Core.Relations;
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
    public class SemesterRepo : ISemesterRepo
    {
        private readonly EduCreditContext _dbcontext;

        public SemesterRepo(EduCreditContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //public async Task<bool> AssignCoursesToSemester(Guid semesterId, List<Guid> courseIds)
        //{
        //    var newSemesterCourses = courseIds.Select(courseId => new Schedule
        //    {
        //        SemesterId = semesterId,
        //        CourseId = courseId
        //    }).ToList();

        //    await _dbcontext.Schedules.AddRangeAsync(newSemesterCourses);
        //    return await _dbcontext.SaveChangesAsync() > 0;
        //}

        public async Task<Semester?> GetCurrentSemester()
        {
            return await _dbcontext.Semesters
                .Include(s => s.Schedules)
                    .ThenInclude(sc => sc.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                                        && s.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));
        }
    }
}
