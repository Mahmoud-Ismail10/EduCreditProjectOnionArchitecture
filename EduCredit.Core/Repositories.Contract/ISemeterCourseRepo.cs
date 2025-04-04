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
        //Task<IReadOnlyList<Guid>> GetSemesterCoursesBySemesterIdAsync(Guid semesterId);
    }
}
