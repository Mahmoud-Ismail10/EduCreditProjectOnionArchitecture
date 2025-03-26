using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface IEnrollmentTableRepo
    {
        Task<EnrollmentTable?> GetEnrollmentTableByStudentIdAndSemesterIdAsync(Guid studentId, Guid semesterId);
    }
}
