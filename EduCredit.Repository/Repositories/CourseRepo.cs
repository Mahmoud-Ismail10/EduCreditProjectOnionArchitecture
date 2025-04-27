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

        public async Task<IReadOnlyList<Course?>> GetCoursesInCurrentsemester(Guid? DepartmentId, Guid CurrentSemesterId)
        {
              var courses =await _dbcontext.Courses
                .Where(c => c.DepartmentId == DepartmentId&& c.SemesterCourses.Select(s=>s.SemesterId).FirstOrDefault()== CurrentSemesterId)
                .ToListAsync();
            if (courses is null) return null;
            return courses;

        }

        public async Task<IReadOnlyList<Guid>> GetValidCourseIds(List<Guid> courseIds)
        {
            var existingCourseIds = await _dbcontext.Courses
                .Where(c => courseIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            return existingCourseIds;
        }
        //public async Task<IReadOnlyList<Guid>> GetCoursesByEnrollmentTableIdAsync(Guid enrollmentTableId)
        //{
        //    var CoursesIds = await _dbcontext.Enrollments
        //        .Where(e => e.EnrollmentTableId == enrollmentTableId)
        //        .Select(e => e.CourseId)
        //        .ToListAsync();
        //    return CoursesIds;
        //}
        //public async Task<IReadOnlyList<Course>> GetCoursesByTeacherIdAsync(Guid teacherId)
        //{
        //    var courses = await _dbcontext.TeacherSchedules
        //        .Where(ts => ts.TeacherId == teacherId)
        //        .Select(ts => ts.Schedule.Course)
        //        .ToListAsync();
        //    return courses;
        //}

    }
}
