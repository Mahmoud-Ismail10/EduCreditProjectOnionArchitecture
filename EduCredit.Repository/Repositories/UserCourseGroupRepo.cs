using EduCredit.Core.Chat;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class UserCourseGroupRepo : IUserCourseGroupRepo
    {
        private readonly EduCreditContext _dbcontext;

        public UserCourseGroupRepo(EduCreditContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Guid>> GetCourseIdsByUserIdAsync(Guid userId)
        {
            return await _dbcontext.UserCourseGroups
                .Where(x => x.UserId == userId)
                .Select(x => x.CourseId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid courseId)
        {
            return await _dbcontext.UserCourseGroups
                .AnyAsync(x => x.UserId == userId && x.CourseId == courseId);
        }

        public async Task<List<UserCourseGroup>> GetUserCourseGroupsWithChatMessages(Guid userId)
        {
            return await _dbcontext.UserCourseGroups
                .Where(ug => ug.UserId == userId)
                .Include(ug => ug.Course)
                    .ThenInclude(c => c.ChatMessages)
                        .ThenInclude(m => m.Sender)
                .ToListAsync();
        }
    }
}
