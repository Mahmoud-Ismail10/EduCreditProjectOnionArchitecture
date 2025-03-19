using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Models;
using EduCredit.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EduCredit.Repository.Repositories
{
    public class CourseRepo : GenericRepository<Course>, ICourseRepo
    {
        private readonly EduCreditContext _dbcontext;

        public CourseRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IReadOnlyList<Guid>> GetValidCourseIds(List<Guid> courseIds)
        {
            var existingCourseIds = await _dbcontext.Courses
                .Where(c => courseIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            return existingCourseIds;
        }
    }
}
