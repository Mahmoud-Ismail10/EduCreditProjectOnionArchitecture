using EduCredit.Core;
using EduCredit.Core.Chat;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class CourseGroupService : ICourseGroupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<string>> GetUserCourseGroups(Guid userId)
        {
            var courseIds = await _unitOfWork._userCourseGroupRepo.GetCourseIdsByUserIdAsync(userId);
            return courseIds.Select(id => id.ToString());
        }

        public async Task SaveMessageAsync(ChatMessage message)
        {
            await _unitOfWork._chatMessageRepo.AddAsync(message);
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesForCourse(Guid courseId)
        {
            return await _unitOfWork._chatMessageRepo.GetMessagesByCourseIdAsync(courseId);
        }

        public async Task AddUserToCourseGroup(Guid userId, Guid courseId)
        {
            if (!await _unitOfWork._userCourseGroupRepo.ExistsAsync(userId, courseId))
            {
                await _unitOfWork._userCourseGroupRepo.AddAsync(new UserCourseGroup
                {
                    UserId = userId,
                    CourseId = courseId
                });
            }
        }

        public async Task RemoveUserFromCourseGroup(Guid userId, Guid courseId)
        {
            var record = await _unitOfWork._userCourseGroupRepo.GetAsync(userId, courseId);
            if (record != null)
            {
                await _unitOfWork._userCourseGroupRepo.RemoveAsync(record);
            }
        }
    }
}