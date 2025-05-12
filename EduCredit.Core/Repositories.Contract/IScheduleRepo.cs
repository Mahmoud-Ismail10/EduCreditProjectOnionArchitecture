using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface IScheduleRepo
    {
        Task<bool> CheckIfCourseExistsInScheduleAsync(Guid id); // update later
        Task<Schedule?> GetScheduleByCourseIdAndSemesterIdAsync(Guid CourseId, Guid SemesterId);
        Task<IReadOnlyList<Guid>> GetValidScheduleIds(List<Guid> scheduleIds); // update later
        Task<IReadOnlyList<Schedule?>> GetSchedulesByTeacherIdAsync(Guid teacherId, Guid semesterID);
        Task<Schedule?> GetScheduleAsync(Guid CourseId,Guid semesterId);
        Task<List<Schedule?>> GetScheduleByManycoursesAsync(List<Guid> CourseId,Guid semesterId);
    }
}
