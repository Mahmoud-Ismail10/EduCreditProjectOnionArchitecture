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

        public async Task AddAsync(UserCourseGroup userCourseGroup)
        {
            _dbcontext.UserCourseGroups.Add(userCourseGroup);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserCourseGroup userCourseGroup)
        {
            _dbcontext.UserCourseGroups.Remove(userCourseGroup);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<UserCourseGroup?> GetAsync(Guid userId, Guid courseId)
        {
            return await _dbcontext.UserCourseGroups
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);
        }
    }
}
