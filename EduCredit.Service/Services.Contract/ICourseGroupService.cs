using EduCredit.Core.Chat;
using EduCredit.Service.DTOs.ChatDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ICourseGroupService
    {
        Task<ReadMessageDto> CreateMessageDtoAsync(ReadMessageDto readMessageDto);
        Task<List<GroupWithLastMessageDto>> GetUserCourseGroups(Guid userId);
        Task<ReadMessageDto?> SaveMessageAsync(ReadMessageDto readMessageDto);
        Task<IReadOnlyList<ReadMessageDto>> GetMessagesForCourse(Guid courseId);
        Task AddUserToCourseGroup(Guid userId, Guid courseId);
        Task<int> DeleteAllGroupsAsync();
        Task BroadcastMessageAsync(ReadMessageDto messageDto);
    }
}
