using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface ISemesterRepo
    {
        //Task<bool> AssignCoursesToSemester(Guid semesterId, List<Guid> courseIds);
        Task<Semester?> GetCurrentSemester();
    }
}
