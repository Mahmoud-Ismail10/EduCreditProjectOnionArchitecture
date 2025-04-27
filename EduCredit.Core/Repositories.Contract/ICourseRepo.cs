using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface ICourseRepo
    {
        Task<IReadOnlyList<Guid>> GetValidCourseIds(List<Guid> courseIds);
        Task<IReadOnlyList<Course?>> GetCoursesInCurrentsemester(Guid? DepartmentId,Guid CurrentSemesterId);
        //  Task<IReadOnlyList<Guid>> GetCoursesByEnrollmentTableIdAsync(Guid enrollmentTableId);
        //Task<IReadOnlyList<Course>> GetCoursesByTeacherIdAsync(Guid teacherId);
    }
}
