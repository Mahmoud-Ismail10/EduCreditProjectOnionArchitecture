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
    public class TeacherScheduleRepo : GenericRepository<TeacherSchedule>, ITeacherScheduleRepo
    {
        private readonly EduCreditContext _dbcontext;
        public TeacherScheduleRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public async Task<List<Guid>> GetTeacherSchedulesByScheduleIdAsync(Guid courseId, Guid SemesterId)
        {
            var teacherSchedules = await _dbcontext.TeacherSchedules
                .Where(ts => ts.Schedule.CourseId == courseId && ts.Schedule.SemesterId == SemesterId)
                .AsNoTracking()
                .Select(ts => ts.TeacherId)
                .ToListAsync();
            if (teacherSchedules is null) return null;
            return teacherSchedules;
        }
    }
}
