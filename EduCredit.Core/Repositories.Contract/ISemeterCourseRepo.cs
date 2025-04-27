using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface ISemeterCourseRepo
    {
        Task<IReadOnlyList<Guid>> GetSemesterCoursesBySemesterIdAndDepartmentIdAsync(Guid semesterId, Guid? departmentId);
        Task<IReadOnlyList<Course>> GetCoursesByTeacherIdAsync(Guid teacherId,Guid semesterID);
    }
}
