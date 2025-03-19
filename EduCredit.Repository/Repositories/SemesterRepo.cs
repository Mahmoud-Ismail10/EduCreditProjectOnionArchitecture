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

        public async Task<bool> AssignCoursesToSemester(Guid semesterId, List<Guid> courseIds)
        {
            var newSemesterCourses = courseIds.Select(courseId => new SemesterCourse
            {
                SemesterId = semesterId,
                CourseId = courseId
            }).ToList();

            await _dbcontext.SemesterCourses.AddRangeAsync(newSemesterCourses);
            return await _dbcontext.SaveChangesAsync() > 0;
        }
    }
}
