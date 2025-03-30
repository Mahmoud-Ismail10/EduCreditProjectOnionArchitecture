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

        public async Task<Schedule?> GetScheduleByCourseIdAsync(Guid courseId)
        {
            Schedule? schedule = await _dbcontext.Schedules
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
            return schedule;
        }
    }
}
