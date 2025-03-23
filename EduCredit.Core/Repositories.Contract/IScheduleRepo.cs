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
        Task<Schedule?> GetScheduleByIdsAsync(Guid courseId, Guid teacherId);
    }
}
