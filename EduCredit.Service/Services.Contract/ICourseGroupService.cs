using EduCredit.Core.Chat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ICourseGroupService
    {
        Task<IEnumerable<string>> GetUserCourseGroups(Guid userId);
        Task SaveMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetMessagesForCourse(Guid courseId);
        Task AddUserToCourseGroup(Guid userId, Guid courseId);
        Task RemoveUserFromCourseGroup(Guid userId, Guid courseId);
    }
}
