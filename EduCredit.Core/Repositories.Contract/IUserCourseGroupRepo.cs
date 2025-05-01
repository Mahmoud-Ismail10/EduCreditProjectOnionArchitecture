using EduCredit.Core.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface IUserCourseGroupRepo
    {
        Task<IEnumerable<Guid>> GetCourseIdsByUserIdAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId, Guid courseId);
        Task AddAsync(UserCourseGroup userCourseGroup);
        Task RemoveAsync(UserCourseGroup userCourseGroup);
        Task<UserCourseGroup?> GetAsync(Guid userId, Guid courseId);
    }
}
