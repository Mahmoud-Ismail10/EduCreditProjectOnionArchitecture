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
        Task<Schedule?> GetScheduleByCourseIdAsync(Guid courseId);
        Task<IReadOnlyList<Guid>> GetValidScheduleIds(List<Guid> scheduleIds);
    }
}
