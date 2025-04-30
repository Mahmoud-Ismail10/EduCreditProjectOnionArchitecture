using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface ITeacherScheduleRepo
    {
        Task<List<Guid>> GetTeacherSchedulesByScheduleIdAsync(Guid courseId,Guid SemesterId);
    }
}
