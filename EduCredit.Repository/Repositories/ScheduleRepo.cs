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
    public class ScheduleRepo : GenericRepository<Schedule>, IScheduleRepo
    {
        private readonly EduCreditContext _dbcontext;

        public ScheduleRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<bool> CheckIfCourseExistsInScheduleAsync(Guid id)
        {
            var exists = await _dbcontext.Schedules
                .AnyAsync(c => c.CourseId == id);
            return exists;
        }

        //public async Task<Schedule?> GetScheduleAsync(Guid CourseId, Guid SemesterId)
        //{
        //    Schedule? schedule = await _dbcontext.Schedules
        //        .FirstOrDefaultAsync(c => c.CourseId == CourseId && c.SemesterId == SemesterId);
        //    return schedule;
        //}

        public async Task<IReadOnlyList<Guid>> GetValidScheduleIds(List<Guid> scheduleIds)
        {
            var existingScheduleIds = await _dbcontext.Schedules
                .Where(c => scheduleIds.Contains(c.CourseId))
                .Select(c => c.CourseId)
                .ToListAsync();

            return existingScheduleIds;
        }

        public async Task<IReadOnlyList<Schedule?>> GetSchedulesByTeacherIdAsync(Guid teacherId, Guid semesterID)
        {
            var courses = _dbcontext.Schedules
                .Where(sc => sc.SemesterId == semesterID)
                .Where(t => t.Course.Schedules.SelectMany(s => s.TeacherSchedules).Where(s => s.TeacherId == teacherId).Any())
                .Include(E => E.Course.Enrollments)
                .ToListAsync();
            if (courses is null) return null;
            return await courses;
        }
    }
}
